using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YASHOP.DAL.DTO.Request;
using YASHOP.DAL.DTO.Response;

namespace YASHOP.BLL.Service.Interfaces
{
    public interface IManageUsers
    {
        Task<List<UserListResponse>> GetUsersAsync();
        Task<UserDetailsResponse> GetUserDetailsAsync();
        Task<BaseResponse> BlockedUserAsync(string userId);
        Task<BaseResponse> UnBlockedUserAsync(string userId);
        Task<BaseResponse> ChangeUserRoleAsync(ChangeUserRoleRequest request);
    }
}
