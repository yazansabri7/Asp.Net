using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using YASHOP.BLL.Service;
using YASHOP.DAL.Data;
using YASHOP.DAL.DTO.Request;
using YASHOP.DAL.DTO.Response;
using YASHOP.DAL.Models;
using YASHOP.DAL.Repository;
using YASHOP.PL.Resourses;

namespace YASHOP.PL.Controllers
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
        [HttpPost("")]
        public IActionResult Create(CategoryRequest request)
        {
            var category = categoryService.CreateCategory(request);
            return Ok(new { message = localizer["Success"].Value });

        }

    }
}
