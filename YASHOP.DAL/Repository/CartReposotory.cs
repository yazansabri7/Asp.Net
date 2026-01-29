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
    public class CartReposotory : ICartRepository
    {
        private readonly ApplicationDbContext context;

        public CartReposotory(ApplicationDbContext context)
        {
            this.context = context;
        }
        public async Task<Cart> CreateAsync(Cart request)
        {
            await context.Carts.AddAsync(request);
            await context.SaveChangesAsync();
            return request;
        }

        public async Task<List<Cart>> GetItemsAsync(string userId)
        {
            var items = await context.Carts
                .Where(c=>c.UserId == userId)
                .Include(c=>c.Product.Translations)
                .ToListAsync();
            return items;
        }
    }
}
