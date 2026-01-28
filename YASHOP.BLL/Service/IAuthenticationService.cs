using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YASHOP.DAL.DTO.Request;
using YASHOP.DAL.DTO.Response;

namespace YASHOP.BLL.Service
{
    public interface IAuthenticationService
    {
        Task<RegisterResponse> RegisterAsync(RegisterRequest registerRequest);
        Task<LoginResponse> LoginAsync(LoginRequest loginRequest);
        Task<bool> ConfirmEmailAsync(string token, string userId);
        Task<ForgetPasswordResponse> RequsetPasswordReset(ForgetPasswordRequest request);
        Task<ResetPasswordResppnse> ResetPassword(ResetPasswordRequest request);
        Task<LoginResponse> RefreshTokenAsync(TokenApiModel request);
    }
}
