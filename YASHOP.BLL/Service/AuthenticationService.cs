using Mapster;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public AuthenticationService(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
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
    }
}
