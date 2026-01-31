using Microsoft.AspNetCore.Http;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YASHOP.DAL.DTO.Request;
using YASHOP.DAL.DTO.Response;
using YASHOP.DAL.Repository;

namespace YASHOP.BLL.Service
{
    public class CheckoutService : ICheckoutService
    {
        private readonly ICartRepository cartRepository;

        public CheckoutService(ICartRepository cartRepository) 
        {
            this.cartRepository = cartRepository;
        }
        public async Task<CheckoutResponse> ProcessPaymentAsync(string userId, CheckoutRequest request , HttpRequest httpRequest)
        {
            var cartItems = await cartRepository.GetItemsAsync(userId);
            if(!cartItems.Any())
            {
                return new CheckoutResponse
                {
                    Success = false,
                    Message = "Cart is empty"
                };
            }
            decimal totalAmount = 0;
            foreach (var item in cartItems)
            {
                if(item.Product.Quantity < item.Count)
                {
                    return new CheckoutResponse
                    {
                        Success = false,
                        Message = $"Insufficient quantity for product {item.Product.Translations.FirstOrDefault(c=>c.Language == "en").Name} "
                    };

                }
                totalAmount += item.Product.Price * item.Count;

            }
            if (request.PaymentMethod == "cash") 
            {
                return new CheckoutResponse
                {
                    Success = true,
                    Message = "Order placed successfully with cash on delivery"
                };
            }
            else if (request.PaymentMethod == "visa")
            {
                var options = new SessionCreateOptions
                {
                    PaymentMethodTypes = new List<string> { "card" },
                    LineItems = new List<SessionLineItemOptions>
            {
               
            },
                    Mode = "payment",
                    SuccessUrl = $"{httpRequest.Scheme}://{httpRequest.Host}/checkout/success",
                    CancelUrl = $"{httpRequest.Scheme}://{httpRequest.Host}/checkout/cancel",
                };

                foreach(var item in cartItems)
                {
                    options.LineItems.Add(new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            Currency = "USD",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = item.Product.Translations.FirstOrDefault(c => c.Language == "en").Name,
                                Description = item.Product.Translations.FirstOrDefault(c => c.Language == "en").Description
                            },
                            UnitAmount = (long)item.Product.Price,
                        },
                        Quantity = item.Count
                    });

                }
                var service = new SessionService();
                var session = service.Create(options);
                return new CheckoutResponse
                {
                    Success = true,
                    Message = "Payment session created",
                    Url = session.Url,
                    PaymentId = session.Id
                };

            }
            else
            {
                return new CheckoutResponse
                {
                    Success = false,
                    Message = "Invalid payment method"
                };
            }
        }
    }
}
