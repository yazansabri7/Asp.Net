using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using YASHOP.BLL.Service;
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
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var result = await authenticationService.RegisterAsync(request);
            return Ok(result);

        }
    }
}
