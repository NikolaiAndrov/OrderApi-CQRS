using Microsoft.EntityFrameworkCore;
using OrderApiCQRS.Data.Models;

namespace OrderApiCQRS.Data
{
    public class WriteDbContext : DbContext
    {
        public WriteDbContext(DbContextOptions<WriteDbContext> options)
            : base(options)
        {
            
        }

        public DbSet<Order> Orders { get; set; }
    }
}
