
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shopping.Models;

namespace Shopping.Infrastructure
{
    public class ShoppingContext : IdentityDbContext<AppUser>
    {
       

        public ShoppingContext(DbContextOptions<ShoppingContext> options) : base(options)

        {

        }

        public DbSet<Page> Pages { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

    }
}
