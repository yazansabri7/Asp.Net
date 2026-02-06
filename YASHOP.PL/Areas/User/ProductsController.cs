using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Security.Claims;
using YASHOP.BLL.Service.Interfaces;
using YASHOP.DAL.DTO.Request;
using YASHOP.PL.Resourses;

namespace YASHOP.PL.Areas.User
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService productService;
        private readonly IStringLocalizer localizer;
        private readonly IReviewService reviewService;

        public ProductsController(IProductService productService
            , IStringLocalizer<SharedResource> localizer
            ,IReviewService reviewService)
        {
            this.productService = productService;
            this.localizer = localizer;
            this.reviewService = reviewService;
        }
        [HttpGet("")]
        public async Task<IActionResult> Index([FromQuery] string lang = "en", [FromQuery] int page =1 , [FromQuery] int limit = 3 , [FromQuery] string? search = null
            , [FromQuery] decimal? maxPrice = null
            , [FromQuery]decimal? minPrice = null
            ,[FromQuery]int? categoryId = null
            , [FromQuery] bool asc = true
            , [FromQuery] string? sortBy = null)
        {
            var response = await productService.GetAllProductsForUserAsync(lang, page, limit,search,categoryId,minPrice,maxPrice ,sortBy,asc);
            return Ok(new {message = localizer["Success"].Value , response });
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> ProductDetails([FromRoute] int id , [FromQuery] string lang = "en")
        {
            var response = await productService.GetProductDetailsForUserAsync(id , lang);
            return Ok(new { message = localizer["Success"].Value, response });
        }
        [HttpGet("categories/{id}")]
        public async Task<IActionResult> GetProductsByCategory([FromRoute] int id,[FromQuery] string lang = "en")
        {
            var response = await productService.GetAllProductsForCategory(id,lang);
            if(response is  null)
            {
            return BadRequest(new { message = localizer["NotFound"].Value  });
            }
            return Ok(new { message = localizer["Success"].Value, response });
        }
        [HttpPost("{id}/review")]
        public async Task<IActionResult> AddReview([FromRoute] int id , [FromBody] CreateReviewRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await reviewService.AddReviewAsync(userId, id, request);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        }
}
