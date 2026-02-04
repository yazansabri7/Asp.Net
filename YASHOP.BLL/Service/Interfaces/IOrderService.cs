using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YASHOP.DAL.DTO.Request;
using YASHOP.DAL.DTO.Response;
using YASHOP.DAL.Models;

namespace YASHOP.BLL.Service.Interfaces
{
    public interface IOrderService
    {
        Task<List<OrderResponse>> GetOrdersAsync(OrderStatus status);
        Task<BaseResponse> UpdateOrderStatusAsync(int orderId , UpdateOrderStatusRequest request);
        Task<Order?> GetOrderByIdAsync(int orderId);
    }
}
