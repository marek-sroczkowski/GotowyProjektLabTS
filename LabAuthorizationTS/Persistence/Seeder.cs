using LabAuthorizationTS.Models.Entities;
using LabAuthorizationTS.Models.Enums;
using Microsoft.AspNetCore.Identity;

namespace LabAuthorizationTS.Persistence
{
    public class Seeder
    {
        private readonly AppDbContext dbContext;
        private readonly IPasswordHasher<User> passwordHasher;

        public Seeder(AppDbContext dbContext, IPasswordHasher<User> passwordHasher)
        {
            this.dbContext = dbContext;
            this.passwordHasher = passwordHasher;
        }

        public void SeedAsync()
        {
            SeedUsers();
            SeedProducts();
        }

        private void SeedProducts()
        {
            var products = new List<Product>
            {
                new Product
                {
                    Name = "Piłka nożna Glider 5 Adidas",
                    Price = 60,
                    UserId = dbContext.Users.FirstOrDefault(u => u.Username == "Seller").Id
                },
                new Product
                {
                    Name = "Hulajnoga Cruiser Moteor",
                    Price = 279.99,
                    UserId = dbContext.Users.FirstOrDefault(u => u.Username == "Seller").Id
                },
                new Product
                {
                    Name = "Xiaomi Redmi Note 9 Pro",
                    Price = 899,
                    UserId = dbContext.Users.FirstOrDefault(u => u.Username == "Seller2").Id
                },
                new Product
                {
                    Name = "Whisky JACK DANIELS",
                    Price = 99.99,
                    OnlyForAdults = true,
                    UserId = dbContext.Users.FirstOrDefault(u => u.Username == "Seller2").Id
                },
            };

            dbContext.Products.AddRange(products);
            dbContext.SaveChanges();
        }

        private void SeedUsers()
        {
            var users = new List<User>
            {
                new User
                {
                    Username = "Admin",
                    Role = UserRole.Admin,
                    BirthDate = DateTime.Now.AddYears(-40)
                },
                new User
                {
                    Username = "Customer",
                    Role = UserRole.Customer,
                    BirthDate = DateTime.Now.AddYears(-30)
                },
                new User
                {
                    Username = "Customer2",
                    Role = UserRole.Customer,
                    BirthDate = DateTime.Now.AddYears(-15)
                },
                new User
                {
                    Username = "Seller",
                    Role = UserRole.Seller,
                    BirthDate = DateTime.Now.AddYears(-35)
                },
                new User
                {
                    Username = "Seller2",
                    Role = UserRole.Seller,
                    BirthDate = DateTime.Now.AddYears(-55)
                }
            };

            users[0].PasswordHash = passwordHasher.HashPassword(users[0], users[0].Username);
            users[1].PasswordHash = passwordHasher.HashPassword(users[1], users[1].Username);
            users[2].PasswordHash = passwordHasher.HashPassword(users[2], users[2].Username);
            users[3].PasswordHash = passwordHasher.HashPassword(users[3], users[3].Username);
            users[4].PasswordHash = passwordHasher.HashPassword(users[4], users[4].Username);

            dbContext.Users.AddRange(users);

            dbContext.SaveChanges();
        }
    }
}