using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YASHOP.DAL.Models;

namespace YASHOP.DAL.Utils
{
    public class UserSeedData : ISeedData
    {
        readonly UserManager<ApplicationUser> userManager;
        public UserSeedData(UserManager<ApplicationUser> userManager) 
        {
            this.userManager = userManager;
        }
        public async Task DataSeed()
        {
            if (! await userManager.Users.AnyAsync())
            {
                var user1 = new ApplicationUser
                {
                    FullName="Yazan Sabri",
                    Email= "y@gmail.com",
                    UserName= "yazan123",
                    EmailConfirmed= true,
                };
                var user2 = new ApplicationUser
                {
                    FullName = "yamen Sabri",
                    Email = "ya@gmail.com",
                    UserName = "yamen123",
                    EmailConfirmed = true,
                };
                var user3 = new ApplicationUser
                {
                    FullName = "yahya Sabri",
                    Email = "yah@gmail.com",
                    UserName = "yahya123",
                    EmailConfirmed = true,
                };
                await userManager.CreateAsync(user1,"Yazan@123");
                await userManager.CreateAsync(user2, "Yamen@123");
                await userManager.CreateAsync(user3,"Yahya@123");

                await userManager.AddToRoleAsync(user1, "SuperAdmin");
                await userManager.AddToRoleAsync(user2, "Admin");
                await userManager.AddToRoleAsync(user3, "User");
            }
        }
    }
}
