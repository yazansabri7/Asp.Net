using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Threading.Tasks;
using YASHOP.BLL.Service.Interfaces;
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
        public async Task<IActionResult> Index([FromQuery] string lang = "en")
        {
            var response = await categoryService.GetAllCategoriesForUser(lang);
            return Ok(new { message = localizer["Success"].Value, response });
        }

    }
}
