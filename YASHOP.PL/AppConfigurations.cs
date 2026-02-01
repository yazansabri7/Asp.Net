using Microsoft.AspNetCore.Identity.UI.Services;
using YASHOP.BLL.Service.Clasess;
using YASHOP.BLL.Service.Interfaces;
using YASHOP.DAL.Repository;
using YASHOP.DAL.Utils;

namespace YASHOP.PL
{
    public static class AppConfigurations
    {
        public static void Config(IServiceCollection Services)
        {
            Services.AddScoped<ICategoryRepository, CategoryRepository>();
            Services.AddScoped<ICategoryService, CategoryService>();
            Services.AddScoped<IAuthenticationService, AuthenticationService>();
            Services.AddScoped<ISeedData, RoleSeedData>();
            Services.AddScoped<ISeedData, UserSeedData>();
            Services.AddTransient<IEmailSender, EmailSender>();
            Services.AddTransient<IFileService , FileService>();
            Services.AddScoped<IProductService, ProductService>();
            Services.AddScoped<IProductRepository, ProductRepository>();
            Services.AddScoped<ITokenService, TokenService>();
            Services.AddScoped<ICartRepository, CartReposotory>();
            Services.AddScoped<ICartService, CartService>();
            Services.AddScoped<ICheckoutService, CheckoutService>();
            Services.AddScoped<IOrderRepository, OrderRepository>();
            Services.AddScoped<IOrderItemRepository, OrderItemRepository>();
        }
    }
}
