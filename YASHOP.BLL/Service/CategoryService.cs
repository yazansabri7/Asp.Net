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
            var categoryRequest= categoryRepository.Adapt<Category>();
            var Request = categoryRepository.Create(categoryRequest);
            return Request.Adapt<CategoryResponse>();
        }
    }
}
