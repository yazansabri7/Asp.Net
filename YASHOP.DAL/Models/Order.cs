using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YASHOP.DAL.Models
{
    public enum OrderStatus
    {
        Pending=1,
        Cancelled=2,
        Approved=3,
        Shipped=4,
        Delivered=5
    }
    public enum PaymentMethod
    {
        Cash=1,
        Visa=2
    }
    public class Order
    {
        public int Id { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public DateTime? ShippedDate { get; set; }

        public PaymentMethod PaymentMethod { get; set; }
        public string? SessionId { get; set; }
        public string? PaymentId { get; set; }
        public decimal? AmountPaid { get; set; }
        public ApplicationUser User { get; set; }
        public string UserId { get; set; }
        public List<OrderItem> OrderItems { get; set; }
    }
}
