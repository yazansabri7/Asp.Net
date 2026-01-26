using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YASHOP.DAL.DTO.Request;
using YASHOP.DAL.DTO.Response;

namespace YASHOP.BLL.Service
{
    public interface IProductService
    {
        Task<ProductResponse> CreateProduct(ProductRequest request);
        Task<List<ProductResponse>> GetAllProductsForAdminAsync();
    }
}
