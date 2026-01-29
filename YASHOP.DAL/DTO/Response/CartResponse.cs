using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YASHOP.DAL.DTO.Response
{
    public class CartResponse
    {
        public int PoductId { get; set; }
        public int Count { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice => Count * Price;
    }
}
