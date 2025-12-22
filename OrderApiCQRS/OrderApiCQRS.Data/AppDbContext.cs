using Microsoft.EntityFrameworkCore;
using OrderApiCQRS.Data.Models;

namespace OrderApiCQRS.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions dbContextOptions)
            : base(dbContextOptions)
        {
            
        }

        public DbSet<Order> Orders { get; set; }
    }
}
