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

        public async Task<bool> DecreaseQuantityForProduct(List<(int productId , int quantity)> items)
        {
            var productIds = items.Select(i=>i.productId).ToList();
            var products = await context.Products.Where(p=> productIds.Contains(p.Id)).ToListAsync();
            foreach(var product in products)
            {
                var item = items.FirstOrDefault(i => i.productId == product.Id);
                if(product.Quantity < item.quantity)
                {
                    return false;
                }
                    product.Quantity -= item.quantity;
            }
            product.Quantity -= quantity;
            await context.SaveChangesAsync();
            return true;

        }
    }
}
