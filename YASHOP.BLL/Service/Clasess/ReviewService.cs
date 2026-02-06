using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YASHOP.BLL.Service.Interfaces;
using YASHOP.DAL.DTO.Request;
using YASHOP.DAL.DTO.Response;
using YASHOP.DAL.Models;
using YASHOP.DAL.Repository;

namespace YASHOP.BLL.Service.Clasess
{
    public class ReviewService : IReviewService
    {
        private readonly IOrderRepository orderRepository;
        private readonly IReviewRepository reviewRepository;

        public ReviewService(IOrderRepository orderRepository , IReviewRepository reviewRepository)
        {
            this.orderRepository = orderRepository;
            this.reviewRepository = reviewRepository;
        }
        public async Task<BaseResponse> AddReviewAsync(string userId, int productId, CreateReviewRequest request)
        {
            var hasDeliverd = await orderRepository.HasUserDeliverdOrderForProduct(userId, productId);
            if (!hasDeliverd)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message="you can only review product you have received"
                };
            }
            var alreadyReview = await reviewRepository.HasUserReviewProduct(userId, productId);
            if (alreadyReview)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message="cant add review"

                };
            }
            var review = request.Adapt<Review>();
            review.UserId = userId;
            review.ProductId = productId;
            await reviewRepository.AddAsync(review);
            return new BaseResponse
            {
                Success=true,
                Message="Review Added Successfully"
            };
        }
    }
}
