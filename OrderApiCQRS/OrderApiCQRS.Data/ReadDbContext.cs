using Microsoft.EntityFrameworkCore;
using OrderApiCQRS.Data.Models;

namespace OrderApiCQRS.Data
{
    public class ReadDbContext : DbContext
    {
        public ReadDbContext(DbContextOptions<ReadDbContext> options)
            : base(options)
        {
            
        }

        public DbSet<Order> Orders { get; set; }
    }
}
