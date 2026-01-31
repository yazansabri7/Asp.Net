using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YASHOP.DAL.DTO.Request;
using YASHOP.DAL.DTO.Response;

namespace YASHOP.BLL.Service
{
    public interface ICheckoutService
    {
        Task<CheckoutResponse> ProcessPaymentAsync(string userId, CheckoutRequest request , HttpRequest httpRequest);
    }
}
