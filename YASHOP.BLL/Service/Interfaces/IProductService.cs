using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YASHOP.DAL.DTO.Request;
using YASHOP.DAL.DTO.Response;

namespace YASHOP.BLL.Service.Interfaces
{
    public interface IProductService
    {
        Task<ProductResponse> CreateProduct(ProductRequest request);
        Task<List<ProductResponse>> GetAllProductsForAdminAsync();
        Task<List<ProductUserResponse>> GetAllProductsForUserAsync(string lang = "en", int page = 1, int limit = 3);
        Task<ProductUserDetails> GetProductDetailsForUserAsync(int id, string lang = "en");
        
    }
}
