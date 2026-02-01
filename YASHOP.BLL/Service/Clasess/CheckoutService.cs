using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Stripe.Checkout;
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
    public class CheckoutService : ICheckoutService
    {
        private readonly ICartRepository cartRepository;
        private readonly IOrderRepository orderRepository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IEmailSender emailSender;
        private readonly IOrderItemRepository orderItemRepository;

        public CheckoutService(ICartRepository cartRepository 
            , IOrderRepository orderRepository
            , UserManager<ApplicationUser> userManager
            , IEmailSender emailSender
            ,IOrderItemRepository orderItemRepository) 
        {
            this.cartRepository = cartRepository;
            this.orderRepository = orderRepository;
            this.userManager = userManager;
            this.emailSender = emailSender;
            this.orderItemRepository = orderItemRepository;
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

            Order order = new Order
            {
                UserId = userId,
                PaymentMethod = request.PaymentMethod,
                AmountPaid = totalAmount

            };
            if (request.PaymentMethod == PaymentMethod.Cash) 
            {
                return new CheckoutResponse
                {
                    Success = true,
                    Message = "Order placed successfully with cash on delivery"
                };
            }
            else if (request.PaymentMethod == PaymentMethod.Visa)
            {
                var options = new SessionCreateOptions
                {
                    PaymentMethodTypes = new List<string> { "card" },
                    LineItems = new List<SessionLineItemOptions>
            {
               
            },
                    Mode = "payment",
                    SuccessUrl = $"{httpRequest.Scheme}://{httpRequest.Host}/api/checkouts/success?session_id={{CHECKOUT_SESSION_ID}}",
                    CancelUrl = $"{httpRequest.Scheme}://{httpRequest.Host}/api/checkouts/cancel",
                    Metadata = new Dictionary<string, string>
                    {
                        {"UserId" , userId },
                    }
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
                            UnitAmount = (long)item.Product.Price * 100,
                        },
                        Quantity = item.Count
                    });

                }
                var service = new SessionService();
                var session = service.Create(options);
                order.SessionId = session.Id;

                await orderRepository.CreateAsync(order);

                return new CheckoutResponse
                {
                    Success = true,
                    Message = "Payment session created",
                    Url = session.Url,
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

        public async Task<CheckoutResponse> HandleSuccessAsync(string sessionId)
        {
            var service = new SessionService();
            var session = service.Get(sessionId);
            var userId = session.Metadata["UserId"];

            var order = await orderRepository.GetBySessionIdAsync(sessionId);
            order.PaymentId = session.PaymentIntentId;
            order.Status = OrderStatus.Approved;
            await orderRepository.UpdateAsync(order);
            var user = await userManager.FindByIdAsync(userId);

            var cartitems = await cartRepository.GetItemsAsync(userId);
            // add Range of order items
            var orderItems = new List<OrderItem>();
            foreach (var item in cartitems)
            {
                var orderItem = new OrderItem
                {
                    OrderId = order.Id,
                    ProductId = item.ProductId,
                    Quantity = item.Count,
                    UnitPrice = item.Product.Price,
                    TotalPrice = item.Product.Price * item.Count,
                };
                orderItems.Add(orderItem);
            }
            await orderItemRepository.CreateRangeAsync(orderItems);
            await cartRepository.ClearCartAsync(userId);

            await emailSender.SendEmailAsync(user.Email, "Order Confirmation", $"Thanks For Trust us {user.UserName}");
            return new CheckoutResponse
            {
                Success = true,
                Message = "Payment successful and order approved",
            };
        }
    }
}
