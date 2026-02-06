using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YASHOP.DAL.Data;
using YASHOP.DAL.Models;

namespace YASHOP.DAL.Repository
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly ApplicationDbContext context;

        public ReviewRepository(ApplicationDbContext context)
        {
            this.context = context;
        }
        public async Task<bool> HasUserReviewProduct(string userId, int productId)
        {
            return await context.Reviews.AnyAsync(r => r.UserId == userId && r.ProductId == productId);
        }
        public async Task<Review> AddAsync(Review request)
        {
            await context.Reviews.AddAsync(request);
            await context.SaveChangesAsync();
            return request;
        }
    }
}
