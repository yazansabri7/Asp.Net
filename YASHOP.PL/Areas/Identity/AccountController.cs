using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using YASHOP.BLL.Service.Interfaces;
using YASHOP.DAL.DTO.Request;

namespace YASHOP.PL.Areas.Identity
{
    [Route("api/auth/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        public IAuthenticationService authenticationService { get; }
        public AccountController(IAuthenticationService authenticationService)
        {
            this.authenticationService = authenticationService;
        }


        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterRequest request )
        {
            var result = await authenticationService.RegisterAsync(request , Request);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);

        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var result = await authenticationService.LoginAsync(request);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string token, string userId)
        {
            var result = await authenticationService.ConfirmEmailAsync(token, userId);
            return Ok(result);
        }
        [HttpPost("SendCode")]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordRequest request)
        {
            var result = await authenticationService.RequsetPasswordReset(request);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpPatch("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest request)
        {
            var result = await authenticationService.ResetPassword(request);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpPatch("RefreshToken")]
        public async Task<IActionResult> RefreshToken(TokenApiModel request)
        {
            var result = await authenticationService.RefreshTokenAsync(request);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
