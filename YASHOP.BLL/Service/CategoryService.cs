using Azure.Core;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YASHOP.DAL.DTO.Request;
using YASHOP.DAL.DTO.Response;
using YASHOP.DAL.Models;
using YASHOP.DAL.Repository;

namespace YASHOP.BLL.Service
{
    public class CategoryService : ICategoryService 
    {
        public ICategoryRepository categoryRepository { get; }
        public CategoryService(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        public List<CategoryResponse> GetAllCategories()
        {
            var categories = categoryRepository.GetAll();
            var response = categories.Adapt<List<CategoryResponse>>();
            return response;
        }

        public CategoryResponse CreateCategory(CategoryRequest category)
        {
            var categoryRequest= category.Adapt<Category>();
            var Request = categoryRepository.Create(categoryRequest);
            return Request.Adapt<CategoryResponse>();
        }

       public async Task<BaseResponse> DeleteCategoryAsync(int id)
        {
            try
            {

                var category = await categoryRepository.FindByIdAsync(id);
                if (category is null)
                {
                    return new BaseResponse()
                    {
                        Success = false,
                        Message = "Category Not Found",
                    };
                }
                    await categoryRepository.DeleteAsync(category);
                    return new BaseResponse()
                    {
                        Success = true,
                        Message = "Category deleted successfully",

                    };
            }catch(Exception ex)
            {
                return new BaseResponse()
                {
                    Success = false,
                    Message = "Can't Delete Category",
                    Errors = new List<string> { ex.Message}
                };
            }
        }
    }
}
