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


        public Task<LoginResponse> LoginAsync(LoginRequest loginRequest)
        {
           throw new NotImplementedException();

        }

        public async Task<RegisterResponse> RegisterAsync(RegisterRequest registerRequest)
        {
            var user = registerRequest.Adapt<ApplicationUser>();
            var result = await userManager.CreateAsync(user, registerRequest.Password);
            if (!result.Succeeded)
            {
                return new RegisterResponse()
                {
                    Message = "Error"

                };
            }
            await userManager.AddToRoleAsync(user, "User");
            return new RegisterResponse()
            {
                Message = "Success"

            };

        }
    }
}
