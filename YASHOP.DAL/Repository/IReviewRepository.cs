using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YASHOP.DAL.Models;

namespace YASHOP.DAL.Repository
{
    public interface IReviewRepository
    {
        Task<bool> HasUserReviewProduct(string userId, int productId);
        Task<Review> AddAsync(Review request);
    }
}
