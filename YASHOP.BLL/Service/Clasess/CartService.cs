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
            var cartItem = await cartRepository.GetCartItemAsync(UserId, request.ProductId);
            var existingCount = cartItem?.Count ?? 0; // if not exit set value to zero
            if(product.Quantity <( existingCount+request.Count))
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Insufficient product quantity"
                };
            }
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

        public async Task<BaseResponse> DeleteProductFromCartAsync(int productId, string userId)
        {
            
            var productDeleted = await cartRepository.GetCartItemAsync(userId, productId);
            if(productDeleted == null)
            {
                return new BaseResponse()
                {
                    Success= false,
                    Message = "Product Not Found"

                };
            }
            await cartRepository.DeleteProductFromCart(productDeleted);
            return new BaseResponse()
            {
                Success = true,
                Message = "Product Deleted Successfully"
            };
        }
        public async Task<BaseResponse> UpdateQuantityAsync(string userId,int productId , int count)
        {
            var cartItem = await cartRepository.GetCartItemAsync(userId , productId);
            var product = await productRepository.FindByIdAsync(productId);
            if (count <= 0)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Invalid Count"
                };
            }
            if(product.Quantity < count)
            {
                return new BaseResponse()
                {
                    Success = false,
                    Message = "Not enough stock"
                };
            }
            cartItem.Count = count;
            await cartRepository.UpdateAsync(cartItem);
            return new BaseResponse
            {
                Success = true,
                Message = "Update Quantity Successfully"
            };
        }
    }
}
