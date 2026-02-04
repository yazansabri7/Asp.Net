using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YASHOP.BLL.Service.Interfaces;
using YASHOP.DAL.DTO.Request;
using YASHOP.DAL.DTO.Response;
using YASHOP.DAL.Models;
using YASHOP.DAL.Repository;

namespace YASHOP.BLL.Service.Clasess
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            this.orderRepository = orderRepository;
        }
        public async Task<Order?> GetOrderByIdAsync(int orderId)
        {
            var order = await orderRepository.GetOrderById(orderId);
            return order;
        }

        public async Task<List<OrderResponse>> GetOrdersAsync(OrderStatus status)
        {
            var orders = await orderRepository.GetOrdersByStatusAsync(status);
            var response = orders.Adapt<List<OrderResponse>>();
            return response;
        }

        public async Task<BaseResponse> UpdateOrderStatusAsync(int orderId, UpdateOrderStatusRequest request)
        {
            var order =await orderRepository.GetOrderById(orderId);
            order.Status = request.NewStatus;
            if(request.NewStatus == OrderStatus.Delivered)
            {
                order.PaymentStatus = PaymentStatus.Paid;
            }
            //else if(newStatus == OrderStatus.Cancelled)
            //{
            //    if(order.Status == OrderStatus.Shipped)
            //    {
            //        return new BaseResponse
            //        {
            //            Success = false,
            //            Message = "cant Cancel This Order"
            //        };
            //    }
            //}
            await orderRepository.UpdateAsync(order);
            return new BaseResponse
            {
                Success = true,
                Message = $"Order Status Updated To {order.Status} Successfully"
            };
        }
    }
}
