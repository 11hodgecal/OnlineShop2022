using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlineShop2022.Models;

namespace OnlineShop2022.Data
{
    public class AppDbContext : IdentityDbContext<CustomUserModel>
    {
        public DbSet<ProductModel> Products { get; set; }

        public DbSet<CategoryModel> Categories { get; set; }
        public DbSet<ShoppingCartItemModel> ShoppingCartItems { get; set; }
        public DbSet<OrderModel> Orders { get; set; }
        public DbSet<RefundModel> refunds { get; set; }
        public DbSet<OrderDetailModel> OrderDetails { get; set; }
        public AppDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
    

            base.OnModelCreating(builder);
            SeedAdmin(builder);
            SeedRoles(builder);
            SeedUserRoles(builder);
        }

        private void SeedRoles(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole()
                {
                    Id = "78bf8cbe-1f70-4d6d-890b-247bc57e6150",
                    Name = "Admin",
                    NormalizedName = "Admin".ToUpper(),
                    ConcurrencyStamp = "7b483dfe-e56c-4d5b-97cd-b32652794d29"

                }
                );

            builder.Entity<IdentityRole>().HasData(
               new IdentityRole()
               {
                   Id = "ecfbe7ad-bb6b-49e6-ac2b-6359a73fbf02",
                   Name = "Customer",
                   NormalizedName = "Customer".ToUpper(),
                   ConcurrencyStamp = "d4e41d27-8605-4e69-8587-2636ed98e286"

               }
               );  
            
            builder.Entity<IdentityRole>().HasData(
               new IdentityRole()
               {
                   Id = "709a40af-4a4e-40b6-887b-d30dcdf07030",
                   Name = "Manager",
                   NormalizedName = "Manager".ToUpper(),
                   ConcurrencyStamp = "db72e6db-01bf-432b-8675-1d08242bb162"

               }
               );
        }


        private void SeedAdmin(ModelBuilder builder)
        {
            PasswordHasher<CustomUserModel> hasher = new PasswordHasher<CustomUserModel>();
            CustomUserModel user = new CustomUserModel();
            user.Id = "27b9af34-a133-43e2-8dd2-aef04ddb2b8c";
            user.UserName = "admin@admin.com";
            user.NormalizedUserName = "admin@admin.com".ToUpper();
            user.NormalizedEmail = "admin@admin.com".ToUpper();
            user.Email = "admin@admin.com";
            user.LockoutEnabled = false;
            user.Fname = "Admin";
            user.Sname = "Admin";
            user.ConcurrencyStamp = "7b483dfe-e56c-4d5b-97cd-b32652794d29";
            user.PasswordHash = hasher.HashPassword(user, "Admin123!");

            builder.Entity<CustomUserModel>().HasData(user);

        }

        private void SeedUserRoles(ModelBuilder builder)
        {
            builder.Entity<IdentityUserRole<string>>().HasData(

                new IdentityUserRole<string>()
                {
                    RoleId = "78bf8cbe-1f70-4d6d-890b-247bc57e6150",
                    UserId = "27b9af34-a133-43e2-8dd2-aef04ddb2b8c"
                });

            builder.Entity<IdentityUserRole<string>>().HasData(

             new IdentityUserRole<string>()
             {
                 RoleId = "ecfbe7ad-bb6b-49e6-ac2b-6359a73fbf02",
                 UserId = "27b9af34-a133-43e2-8dd2-aef04ddb2b8c"
             });
                   
            builder.Entity<IdentityUserRole<string>>().HasData(

             new IdentityUserRole<string>()
             {
                 RoleId = "709a40af-4a4e-40b6-887b-d30dcdf07030",
                 UserId = "27b9af34-a133-43e2-8dd2-aef04ddb2b8c"
             });

        }
    }
}
