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
    }
}
