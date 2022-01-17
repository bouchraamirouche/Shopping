using Microsoft.EntityFrameworkCore;
using Shopping.Models;

namespace Shopping.Infrastructure
{
    public class ShoppingContext : DbContext
    {
        public ShoppingContext(DbContextOptions<ShoppingContext> options) : base(options)

    {

    }

    public DbSet<Page> Pages { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
    }
}
