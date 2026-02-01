using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YASHOP.DAL.Models
{
    [PrimaryKey(nameof(OrderId), nameof(ProductId))]
    public class OrderItem
    {
        public Order Order { get; set; }
        public int OrderId { get; set; }
        public Product Product { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
