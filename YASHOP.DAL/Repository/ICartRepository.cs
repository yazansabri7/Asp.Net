using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YASHOP.DAL.Models;

namespace YASHOP.DAL.Repository
{
    public interface ICartRepository
    {
        Task<Cart> CreateAsync(Cart request);
        Task<List<Cart>> GetItemsAsync(string userId);
        Task<Cart?> GetCartItemAsync(string userId, int productId);
        Task<Cart> UpdateAsync(Cart cart);
        Task DeleteCartAsync(string userId);
    }
}
