using Mapster;
using Microsoft.AspNetCore.Identity;
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
    public class CartService : ICartService
    {
        private readonly ICartRepository cartRepository;
        private readonly IProductRepository productRepository;
        private readonly UserManager<ApplicationUser> userManager;

        public CartService(ICartRepository cartRepository , IProductRepository productRepository ,
            UserManager<ApplicationUser> userManager) 
        {
            this.cartRepository = cartRepository;
            this.productRepository = productRepository;
            this.userManager = userManager;
        }
        public async Task<BaseResponse> AddToCartAsync(string UserId, AddToCartRequest request)
        {
            var product = await productRepository.FindByIdAsync(request.ProductId);
            if (product == null)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Product not found"
                };
            }
            if(product.Quantity < request.Count)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Insufficient product quantity"
                };
            }
            var cartItem = await cartRepository.GetCartItemAsync(UserId, request.ProductId);
            if(cartItem is not null)
            {
                cartItem.Count = cartItem.Count + request.Count;
                await cartRepository.UpdateAsync(cartItem);
            }
            else
            {
                    var cart = request.Adapt<Cart>();
                cart.UserId = UserId;
                await cartRepository.CreateAsync(cart);
            }
                return new BaseResponse
                {
                    Success = true,
                    Message = "Product added to cart successfully"
                };

        }

        public async Task<CartSummaryResponse> GetItemsAsync(string userId, string lang = "en")
        {
            var user = await userManager.FindByIdAsync(userId);
            if(user == null)
            {
                return null;
            }
            var cartItems = await cartRepository.GetItemsAsync(userId);
            var items = cartItems.Select(c=>new CartResponse
            {
                PoductId = c.ProductId,
                ProductName = c.Product.Translations.FirstOrDefault(t=>t.Language==lang).Name,
                Description = c.Product.Translations.FirstOrDefault(t=>t.Language==lang).Description,
                Count = c.Count,
                Price = c.Product.Price,

            }).ToList();
            return new CartSummaryResponse{
                Items = items
            };
            
        }
        public async Task<BaseResponse> ClearCartAsync(string userId)
        {
            
            await cartRepository.ClearCartAsync(userId);
            return new BaseResponse
            {
                Success = true,
                Message = "Cart cleared successfully"
            };

        }
    }
}
