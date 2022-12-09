using LabAuthorizationTS.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace LabAuthorizationTS.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}