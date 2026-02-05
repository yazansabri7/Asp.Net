using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YASHOP.BLL.Service.Interfaces;
using YASHOP.DAL.DTO.Request;
using YASHOP.DAL.DTO.Response;
using YASHOP.DAL.Models;

namespace YASHOP.BLL.Service.Clasess
{
    public class ManageUsers : IManageUsers
    {
        private readonly UserManager<ApplicationUser> userManager;

        public ManageUsers(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }
        public async Task<BaseResponse> BlockedUserAsync(string userId)
        {
            var user= await userManager.FindByIdAsync(userId);
            if(user is null)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message="User Not found"
                };
            }
            await userManager.SetLockoutEnabledAsync(user, true);
            await userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);
            await userManager.UpdateAsync(user);
            return new BaseResponse
            {
                Success=true,
                Message=$"User {user.UserName} is Blocked Successfully"
            };
        }

        public async Task<BaseResponse> ChangeUserRoleAsync(ChangeUserRoleRequest request)
        {
            var user = await userManager.FindByIdAsync(request.UserId);
            var currentRoles = await userManager.GetRolesAsync(user);
            await userManager.RemoveFromRolesAsync(user, currentRoles);
            await userManager.AddToRolesAsync(user,new List<string> { request.Role });
            return new BaseResponse
            {
                Success = true,
                Message = "User Updated Role Successfully"
            };
        }

        public Task<UserDetailsResponse> GetUserDetailsAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<List<UserListResponse>> GetUsersAsync()
        {
            var users = await userManager.Users.ToListAsync();
            var respone = users.Adapt<List<UserListResponse>>();
            for(int i =0; i<users.Count; i++)
            {
                var roles = await userManager.GetRolesAsync(users[i]);
                respone[i].Roles = roles.ToList();
                
            }
            return respone;
        }

        public async Task<BaseResponse> UnBlockedUserAsync(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user is null)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "User Not found"
                };
            }
            await userManager.SetLockoutEnabledAsync(user, false);
            await userManager.SetLockoutEndDateAsync(user, null);
            await userManager.UpdateAsync(user);
            return new BaseResponse
            {
                Success = true,
                Message = $"User {user.UserName} is UnBlocked Successfully"
            };
        }
    }
}
