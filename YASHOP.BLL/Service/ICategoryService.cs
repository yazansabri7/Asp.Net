using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YASHOP.DAL.DTO.Request;
using YASHOP.DAL.DTO.Response;
using YASHOP.DAL.Models;

namespace YASHOP.BLL.Service
{
    public interface ICategoryService
    {
        
       List<CategoryResponse> GetAllCategories();
       CategoryResponse CreateCategory(CategoryRequest category);
       Task<BaseResponse> DeleteCategoryAsync(int id);
       Task<BaseResponse> UpdateCategoryAsync(CategoryRequest request, int id);


    }
}
