using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using YASHOP.BLL.Service.Interfaces;
using YASHOP.DAL.DTO.Request;
using YASHOP.DAL.DTO.Response;
using YASHOP.DAL.Models;

namespace YASHOP.BLL.Service.Clasess
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ITokenService tokenService;

        public UserManager<ApplicationUser> userManager { get; }
        public IEmailSender emailSender { get; }
        public SignInManager<ApplicationUser> signInManager { get; }

        public AuthenticationService(UserManager<ApplicationUser> userManager , ITokenService tokenService ,
            IEmailSender emailSender , 
            SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.tokenService = tokenService;
            this.emailSender = emailSender;
            this.signInManager = signInManager;
        }


        public async Task<LoginResponse> LoginAsync(LoginRequest loginRequest)
        {
            try
            {
                var user = await userManager.FindByEmailAsync(loginRequest.Email);
                if(user is null)
                {
                    return new LoginResponse()
                    {
                        Success = false,
                        Message="Invalid Email",
                    };
                }
                if (await userManager.IsLockedOutAsync(user))
                {
                    return new LoginResponse()
                    {
                        Success = false,
                        Message = "User is Locked Out , try again later",
                    };

                }
                var result= await signInManager.CheckPasswordSignInAsync(user, loginRequest.Password, true);
                if(result.IsLockedOut)
                {
                    return new LoginResponse()
                    {
                        Success = false,
                        Message = "Account is Locked Out , try again later",
                    };
                }
                if(result.IsNotAllowed)
                {
                    return new LoginResponse()
                    {
                        Success = false,
                        Message = "Email is not confirmed",
                    };
                }

                if (!result.Succeeded)
                {
                    return new LoginResponse()
                    {
                        Success = false,
                        Message = "Invalid Password",
                    };
                }
                var accessToken = await tokenService.GenerateAccessToken(user);
                var refreshToken = tokenService.GenerateRefreshToken();
                user.RefreshToken = refreshToken;
                user.RefreshTokenExpireTime = DateTime.UtcNow.AddDays(7);

                await userManager.UpdateAsync(user);

                return new LoginResponse()
                {
                    Success = true,
                    Message = "Login Successfully",
                    AccessToken = accessToken,
                    RefreshToken = refreshToken
                };
            }
            catch(Exception ex)
            {
                return new LoginResponse()
                {
                    Success = false,
                    Message = "An Exception Error",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<RegisterResponse> RegisterAsync(RegisterRequest registerRequest , HttpRequest request)
        {
            try
            {
                var user = registerRequest.Adapt<ApplicationUser>();
                var result = await userManager.CreateAsync(user, registerRequest.Password);
            
                if (!result.Succeeded)
                {
                    return new RegisterResponse()
                    {
                        Message = "Error",
                        Success = false,
                        Errors = result.Errors.Select(e => e.Description).ToList()

                    };
                }
                await userManager.AddToRoleAsync(user, "User");
                var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                token = Uri.EscapeDataString(token);
                var EmailUrl = $"{request.Scheme}//{request.Host}/api/auth/Account/ConfirmEmail?token={token}&userId={user.Id}";
                await emailSender.SendEmailAsync(user.Email , "Welcome" , $"<h1> Welcome .. {user.UserName} </h1> " +
                    $"<a href='{EmailUrl}'>Confirm Email</a>");
                return new RegisterResponse()
                {
                    Message = "Success",
                    Success = true

                };
            }catch(Exception ex)
            {
                return new RegisterResponse()
                {

                    Message = "An Exception Error",
                    Success = false,
                    Errors = new List<string>{ ex.Message }
                };
            }
        }
        public async Task<bool> ConfirmEmailAsync(string token, string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user is null)
            {
                return false;
            }
            var result = await userManager.ConfirmEmailAsync(user, token);
            if(!result.Succeeded)
            {
                return false;
            }
            return true;
        }
       
        public async Task<ForgetPasswordResponse> RequsetPasswordReset(ForgetPasswordRequest request)
        {
            var user = await userManager.FindByEmailAsync(request.Email);
            if(user is null)
            {
                return new ForgetPasswordResponse()
                {
                    Success = false,
                    Message = "Invalid Email"
                };
            }
            var random = new Random();
            var code = random.Next(100000, 999999).ToString();
            user.CodeResetPassword = code;
            user.ExpireCodeResetPassword = DateTime.UtcNow.AddMinutes(15);
            await userManager.UpdateAsync(user);
            await emailSender.SendEmailAsync(user.Email, "Reset Password", $"<p>Your Reset Code is : {code} </p>");
            return new ForgetPasswordResponse()
            {
                Success = true,
                Message = "Reset Code Sent to your Email"
            };


        }

        public async Task<ResetPasswordResppnse> ResetPassword(ResetPasswordRequest request)
        {
            var user = await userManager.FindByEmailAsync(request.Email);
            if (user is null)
            {
                return new ResetPasswordResppnse()
                {
                    Success = false,
                    Message = "Invalid Email"
                };
            }
            else if(user.CodeResetPassword != request.CodeResetPassword)
            {
                return new ResetPasswordResppnse()
                {
                    Success = false,
                    Message = "Invalid Reset Code"
                };
            }
            else if(user.ExpireCodeResetPassword < DateTime.UtcNow)
            {
                return new ResetPasswordResppnse()
                {
                    Success = false,
                    Message = "Reset Code Expired"
                };
            }
            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            var result = await userManager.ResetPasswordAsync(user, token , request.NewPassword);
            if(!result.Succeeded)
            {
                return new ResetPasswordResppnse()
                {
                    Success = false,
                    Message = "Error",
                    Errors = result.Errors.Select(e => e.Description).ToList()
                };
            }
            await emailSender.SendEmailAsync(user.Email, "Password Reset Successfully", $"<p>Your Password has been reset successfully.</p>");
            return new ResetPasswordResppnse()
            {
                Success = true,
                Message = "Password Reset Successfully"
            };

        }

        public async Task<LoginResponse> RefreshTokenAsync(TokenApiModel request)
        {
            var accessToken = request.AccessToken;
            var refreshToken = request.RefreshToken;
            var principal = tokenService.GetPrincipalFromExpiredToken(accessToken);
            var userName = principal.Identity.Name;

            //var user = await userManager.FindByNameAsync(userName);
            var user = await userManager.Users.FirstOrDefaultAsync(u => u.UserName == userName);
            if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpireTime <= DateTime.UtcNow)
            {
                return new LoginResponse()
                {
                    Success = false,
                    Message = "Invalid User Request"
                };
            }
            var newAccessToken = await tokenService.GenerateAccessToken(user);
            var newRefreshToken = tokenService.GenerateRefreshToken();
            user.RefreshToken = newRefreshToken;
            await userManager.UpdateAsync(user);
            return new LoginResponse()
            {
                Success = true,
                Message = "Refreshed Token Successfully",
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            };
        }
     }
}
