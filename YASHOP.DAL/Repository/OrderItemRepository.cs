using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YASHOP.DAL.Data;
using YASHOP.DAL.Models;

namespace YASHOP.DAL.Repository
{
    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly ApplicationDbContext context;

        public OrderItemRepository(ApplicationDbContext context) 
        {
            this.context = context;
        }
        public async Task CreateRangeAsync(List<OrderItem> request)
        {
            await context.OrderItems.AddRangeAsync(request);
            await context.SaveChangesAsync();
          
        }
    }
}
