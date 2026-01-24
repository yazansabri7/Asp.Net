using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Threading.Tasks;
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
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var response = await categoryService.GetAllCategoriesForAdmin();
            return Ok(response);
        }


        [HttpPost("")]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryRequest request)
        {
            var response = await categoryService.CreateCategory(request);
            return Ok(new { message = localizer["Success"].Value});
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory([FromRoute] int id)
        {
            var response = await categoryService.DeleteCategoryAsync(id);
            if (!response.Success)
            {
                if(response.Message.Contains("Not Found"))
                {
                    return NotFound(response);
                }
                return BadRequest(response);
            }
            return Ok(response);
        }
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateCategory([FromBody] CategoryRequest request , [FromRoute] int id)
        {
            var response = await categoryService.UpdateCategoryAsync(request, id);
            if(!response.Success)
            {
                if (response.Message.Contains("Not Found"))
                {
                    return NotFound(response);
                }
                return BadRequest(response);
            }
            return Ok(response);
        }
        [HttpPatch("toggle-status/{id}")]
        public async Task<IActionResult> ToggleStatus([FromRoute] int id)
        {
            var response = await categoryService.ToggleStatusAsync(id);
            if (!response.Success)
            {
                if (response.Message.Contains("Not Found"))
                {
                    return NotFound(response);
                }
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
