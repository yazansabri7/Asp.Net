using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YASHOP.DAL.Data;
using YASHOP.DAL.Models;

namespace YASHOP.DAL.Repository
{
    public class ProductRepository : IProductRepository
    {
        public ApplicationDbContext context { get; }
        public ProductRepository(ApplicationDbContext context)  
        {
            this.context = context;
        }
        public async Task<Product> AddAsync(Product request)
        {
             await context.Products.AddAsync(request);
            await context.SaveChangesAsync();
            return request;
        }

        public async Task<List<Product>> GetAllForAdminAsync()
        {
            return await context.Products.Include(p=>p.Translations).Include(p=>p.User).ToListAsync();
        }
        public async Task<List<Product>> GetAllForUserAsync()
        {
            return await context.Products.Include(p=>p.Translations).ToListAsync();
        }
        public async Task<Product?> FindByIdAsync(int id)
        {
            return await context.Products.Include(p => p.Translations).Include(p => p.SubImages)
                 .FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}
