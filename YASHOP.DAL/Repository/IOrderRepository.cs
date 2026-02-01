using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YASHOP.DAL.Models;

namespace YASHOP.DAL.Repository
{
    public interface IOrderRepository
    {

        Task<Order> CreateAsync(Order request);
        Task<Order> GetBySessionIdAsync(string sessionId);
        Task<Order> UpdateAsync(Order order);

    }
}
