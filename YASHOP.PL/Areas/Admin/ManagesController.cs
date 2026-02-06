using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YASHOP.BLL.Service.Interfaces;
using YASHOP.DAL.DTO.Request;

namespace YASHOP.PL.Areas.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    [Authorize(Roles ="Admin")]
    public class ManagesController : ControllerBase
    {
        private readonly IManageUsers manageUser;

        public ManagesController(IManageUsers manageUser )
        {
            this.manageUser = manageUser;
        }
        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            var response = await manageUser.GetUsersAsync();
            return Ok(response);
        }
        [HttpPatch("block/{id}")]
        public async Task<IActionResult> BlockedUser([FromRoute] string id)
        => Ok(await manageUser.BlockedUserAsync(id));
        [HttpPatch("Unblock/{id}")]
        public async Task<IActionResult> UnBlockedUser([FromRoute] string id)
        => Ok(await manageUser.UnBlockedUserAsync(id));

        [HttpPatch("UpdateRole")]
        [Authorize(Roles ="SuperAdmin")]
        public async Task<IActionResult> UpdateRole([FromBody] ChangeUserRoleRequest request)
        {
            var response = await manageUser.ChangeUserRoleAsync(request);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

    }
}
