using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using YASHOP.BLL.Service.Interfaces;
using YASHOP.DAL.DTO.Request;

namespace YASHOP.PL.Areas.User
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartsController : ControllerBase
    {
        private readonly ICartService cartService;

        public CartsController(ICartService cartService)
        {
            this.cartService = cartService;
        }
        [HttpPost("")]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartRequest request )
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await cartService.AddToCartAsync(userId, request);
            return Ok(result);
        }
        [HttpGet("")]
        public async Task<IActionResult> GetItems()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await cartService.GetItemsAsync(userId);
            return Ok(result);
        }
        [HttpDelete("")]
        public async Task<IActionResult> ClearCart()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await cartService.ClearCartAsync(userId);
            return Ok(result);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductFromCart([FromRoute]int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await cartService.DeleteProductFromCartAsync(id, userId);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);

        }
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdatedCount([FromRoute] int id , [FromBody] UpdateQuantityRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await cartService.UpdateQuantityAsync(userId, id, request.Count);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
