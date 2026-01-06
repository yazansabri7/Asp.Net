using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YASHOP.DAL.Data;
using YASHOP.DAL.DTO.Request;
using YASHOP.DAL.Models;

namespace YASHOP.DAL.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        public ApplicationDbContext context { get; }
        public CategoryRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public List<Category> GetAll()
        {
            var response = context.Categories.Include(c => c.Translations).ToList();
            return response;
        }

        public Category Create(Category request)
        {
            var category = context.Categories.Add(request);
            context.SaveChanges();
            return request;
        }
    }
}
