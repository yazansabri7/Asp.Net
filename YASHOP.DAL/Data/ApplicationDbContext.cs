using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using YASHOP.DAL.Models;

namespace YASHOP.DAL.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryTranslation> CategoryTranslations { get; set; }
        public IHttpContextAccessor httpContextAccessor { get; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options , 
           IHttpContextAccessor httpContextAccessor
            ) : base(options)
        {
            this.httpContextAccessor = httpContextAccessor;
        }
        //Change Tables Names For Identity Table
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ApplicationUser>().ToTable("Users");
            builder.Entity<IdentityRole>().ToTable("Roles");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
            builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");

        }

        public override int SaveChanges()
        {
            var entries = ChangeTracker.Entries<BaseModel>();
            var CurrentUserId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            foreach(var entry in entries)
            {
                if(entry.State == EntityState.Added)
                {
                    entry.Property(x => x.CreatedAt).CurrentValue = DateTime.UtcNow;
                    entry.Property(x => x.CreatedBy).CurrentValue = CurrentUserId;
                }
                else if(entry.State == EntityState.Modified)
                {
                    entry.Property(x => x.UpdatedAt).CurrentValue = DateTime.UtcNow;
                    entry.Property(x => x.UpdatedBy).CurrentValue = CurrentUserId;
                }

            }

            return base.SaveChanges();
        }
    }
}
