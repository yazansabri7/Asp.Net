using Microsoft.AspNetCore.Identity.UI.Services;
using YASHOP.BLL.Service;
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
        }
    }
}
