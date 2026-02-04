using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using YASHOP.BLL.Service.Interfaces;
using YASHOP.DAL.DTO.Request;
using YASHOP.DAL.Models;
using YASHOP.PL.Resourses;

namespace YASHOP.PL.Areas.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    [Authorize(Roles ="Admin")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService orderService;
        private readonly IStringLocalizer<SharedResource> localizer;

        public OrdersController(IOrderService orderService , IStringLocalizer<SharedResource> localizer)
        {
            this.orderService = orderService;
            this.localizer = localizer;
        }
        [HttpGet("")]
        public async Task<IActionResult> GetOrders([FromQuery] OrderStatus status = OrderStatus.Pending)
        {
            var response = await orderService.GetOrdersAsync(status);
            return Ok(response);
        }
        [HttpPatch("{orderId}")]
        public async Task<IActionResult> UpdateStatus([FromRoute] int orderId , [FromBody] UpdateOrderStatusRequest request)
        {
            var response = await orderService.UpdateOrderStatusAsync(orderId, request);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
