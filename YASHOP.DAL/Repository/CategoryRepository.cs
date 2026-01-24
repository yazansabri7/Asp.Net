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

        public async Task<List<Category>> GetAllAsync()
        {
            var response = await context.Categories.Include(c => c.Translations).ToListAsync();
            return response;
        }

        public async Task<Category> CreateAsync(Category request)
        {
            await context.Categories.AddAsync(request);
            await context.SaveChangesAsync();
            return request;
        }

        public async Task<Category?> FindByIdAsync(int id)
        {
            var category =await context.Categories.Include(c => c.Translations).FirstOrDefaultAsync(x => x.Id == id);
            return category;

        }
        public async Task DeleteAsync(Category category)
        {
            context.Categories.Remove(category);
            await context.SaveChangesAsync();
        }
        public async Task<Category?> UpdateAsync(Category category)
        {
            context.Update(category);
            await context.SaveChangesAsync();
            return category;
        }
    }
}
