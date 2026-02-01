using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using YASHOP.BLL.Service.Interfaces;
using YASHOP.DAL.DTO.Request;
using YASHOP.PL.Resourses;

namespace YASHOP.PL.Areas.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService productService;
        private readonly IStringLocalizer<SharedResource> localizer;

        public ProductsController(IProductService productService , IStringLocalizer<SharedResource> localizer)
        {
            this.productService = productService;
            this.localizer = localizer;
        }
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var response = await productService.GetAllProductsForAdminAsync();
            return Ok(response);
        }
        [HttpPost("")]
        public async Task<IActionResult> Create([FromForm] ProductRequest request)
        {
            var response = await productService.CreateProduct(request);
            return Ok(new {message = localizer["Success"].Value , response  });
        }
        
    }
}
