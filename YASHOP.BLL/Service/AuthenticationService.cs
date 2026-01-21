using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using YASHOP.DAL.DTO.Request;
using YASHOP.DAL.DTO.Response;
using YASHOP.DAL.Models;

namespace YASHOP.BLL.Service
{
    public class AuthenticationService : IAuthenticationService
    {
        public UserManager<ApplicationUser> userManager { get; }
        public IConfiguration configuration { get; }
        public IEmailSender emailSender { get; }

        public AuthenticationService(UserManager<ApplicationUser> userManager , IConfiguration configuration , 
            IEmailSender emailSender)
        {
            this.userManager = userManager;
            this.configuration = configuration;
            this.emailSender = emailSender;
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
                var result = await userManager.CheckPasswordAsync(user, loginRequest.Password);
                if (!result)
                {
                    return new LoginResponse()
                    {
                        Success = false,
                        Message = "Invalid Password",
                    };
                }
                return new LoginResponse()
                {
                    Success = true,
                    Message = "Login Successfully",
                    AccessToken = await GenerateAccessToken(user)
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

        public async Task<RegisterResponse> RegisterAsync(RegisterRequest registerRequest)
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
                var EmailUrl = $"https://localhost:7220/api/auth/Account/ConfirmEmail?token={token}&userId={user.Id}";
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




        private async Task<string> GenerateAccessToken(ApplicationUser user)
        {
            var userClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.Name,user.UserName)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: userClaims,
                expires: DateTime.UtcNow.AddMinutes(5),
                signingCredentials: creds 
                );
            return new  JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
