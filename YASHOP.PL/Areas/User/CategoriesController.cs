using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using YASHOP.BLL.Service;
using YASHOP.PL.Resourses;

namespace YASHOP.PL.Areas.User
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        public ICategoryService categoryService { get; }
        public IStringLocalizer<SharedResource> localizer { get; }

        public CategoriesController(ICategoryService categoryService , 
            IStringLocalizer<SharedResource> localizer)
        {
            this.categoryService = categoryService;
            this.localizer = localizer;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            var response = categoryService.GetAllCategories();
            return Ok(new { message = localizer["Success"].Value, response });
        }

    }
}
