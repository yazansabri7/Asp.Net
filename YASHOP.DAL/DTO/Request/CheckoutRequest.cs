using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using YASHOP.DAL.Models;

namespace YASHOP.DAL.DTO.Request
{
    public class CheckoutRequest
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PaymentMethod PaymentMethod { get; set; }  
    }
}
