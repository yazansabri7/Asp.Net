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
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext context;

        public OrderRepository(ApplicationDbContext context)
        {
            this.context = context;
        }
        public async Task<Order> CreateAsync(Order request)
        {
            await context.Orders.AddAsync(request);
            await context.SaveChangesAsync();
            return request;
        }

        public async Task<Order> GetBySessionIdAsync(string sessionId)
        {
            var order = await context.Orders.FirstOrDefaultAsync(o => o.SessionId == sessionId);
            return order;
        }

        public async Task<Order?> GetOrderById(int orderId)
        {
            var order = await context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                .ThenInclude(o => o.Product)
                .FirstOrDefaultAsync(o => o.Id == orderId);
            return order;
        }

        public Task<List<Order>> GetOrdersByStatusAsync(OrderStatus status)
        {
            var orders = context.Orders
                .Include(o => o.User)
                .Where(o => o.Status == status)
                .ToListAsync();
            return orders;
        }
        public async Task<bool> HasUserDeliverdOrderForProduct(string userId,int productId)
        {
            return await context.Orders
                .Where(o => o.UserId == userId && o.Status == OrderStatus.Delivered)
                .SelectMany(o => o.OrderItems)
                .AnyAsync(oi => oi.ProductId == productId);
        }
        public async Task<Order> UpdateAsync(Order order)
        {
            context.Orders.Update(order);
            await context.SaveChangesAsync();
            return order;
        }
    }
}
