using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YASHOP.DAL.DTO.Request;
using YASHOP.DAL.Models;

namespace YASHOP.DAL.Repository
{
    public interface ICategoryRepository
    {
         List<Category> GetAll();
         Category Create(Category request);
         Task<Category?> FindByIdAsync(int id);
         Task DeleteAsync(Category category);
         Task<Category?> UpdateAsync(Category category);
    }
}
