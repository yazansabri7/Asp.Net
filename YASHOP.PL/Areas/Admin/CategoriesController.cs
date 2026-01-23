using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using YASHOP.BLL.Service;
using YASHOP.DAL.DTO.Request;
using YASHOP.PL.Resourses;

namespace YASHOP.PL.Areas.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    [Authorize(Roles ="Admin")]
    public class CategoriesController : ControllerBase
    {
        public ICategoryService categoryService { get; }
        public IStringLocalizer<SharedResource> localizer { get; }

        public CategoriesController(ICategoryService categoryService,
            IStringLocalizer<SharedResource> localizer)
        {
            this.categoryService = categoryService;
            this.localizer = localizer;
        }

        [HttpPost("")]
        public IActionResult Create(CategoryRequest request)
        {
            var response = categoryService.CreateCategory(request);
            return Ok(new { message = localizer["Success"].Value});
        }
    }
}
