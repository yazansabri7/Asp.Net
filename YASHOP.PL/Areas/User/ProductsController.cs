using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using YASHOP.BLL.Service.Interfaces;
using YASHOP.PL.Resourses;

namespace YASHOP.PL.Areas.User
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService productService;
        private readonly IStringLocalizer localizer;

        public ProductsController(IProductService productService , IStringLocalizer<SharedResource> localizer)
        {
            this.productService = productService;
            this.localizer = localizer;
        }
        [HttpGet("")]
        public async Task<IActionResult> Index([FromQuery] string lang = "en")
        {
            var response = await productService.GetAllProductsForUserAsync(lang);
            return Ok(new {message = localizer["Success"].Value , response });
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> ProductDetails([FromRoute] int id , [FromQuery] string lang = "en")
        {
            var response = await productService.GetProductDetailsForUserAsync(id , lang);
            return Ok(new { message = localizer["Success"].Value, response });
        }
    }
}
