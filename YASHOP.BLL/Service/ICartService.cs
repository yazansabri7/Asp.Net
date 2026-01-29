using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YASHOP.DAL.DTO.Request;
using YASHOP.DAL.DTO.Response;

namespace YASHOP.BLL.Service
{
    public interface ICartService
    {
        Task<BaseResponse> AddToCartAsync(string UserId, AddToCartRequest request);
        Task<CartSummaryResponse> GetItemsAsync(string userId , string lang = "en");
    }
}
