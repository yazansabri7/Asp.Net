using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YASHOP.DAL.Models;

namespace YASHOP.DAL.Repository
{
    public interface IProductRepository
    {
        Task<Product> AddAsync(Product request);
        Task<List<Product>> GetAllForAdminAsync();
        Task<List<Product>> GetAllForUserAsync();
        Task<Product?> FindByIdAsync(int id);
        Task<bool> DecreaseQuantityForProduct(List<(int productId, int quantity)> items);
    }
}
