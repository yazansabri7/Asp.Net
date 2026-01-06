using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using YASHOP.DAL.Data;
using YASHOP.DAL.DTO.Request;
using YASHOP.DAL.DTO.Response;
using YASHOP.DAL.Models;
using YASHOP.PL.Resourses;

namespace YASHOP.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        public ApplicationDbContext context { get; }
        public IStringLocalizer<SharedResource> localizer { get; }

        public CategoriesController(ApplicationDbContext context ,
            IStringLocalizer<SharedResource> localizer)
        {
            this.context = context;
            this.localizer = localizer;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            //use Include(c=>c.Translations) because join between category and categorytranslation
            var categories = context.Categories.Include(c=>c.Translations).ToList();
            var response = categories.Adapt<List<CategoryResponse>>();
            return Ok(new { message = localizer["Success"].Value, response });
        }
        [HttpPost("")]
        public IActionResult Create(CategoryRequest request)
        {
            var category = request.Adapt<Category>();
            context.Categories.Add(category);
            context.SaveChanges();
            return Ok(new { message = localizer["Success"].Value });

        }

    }
}
