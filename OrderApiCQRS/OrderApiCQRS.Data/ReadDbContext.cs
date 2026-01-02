using Microsoft.EntityFrameworkCore;
using OrderApiCQRS.Data.Models;
using System.Reflection;

namespace OrderApiCQRS.Data
{
    public class ReadDbContext : DbContext
    {
        public ReadDbContext(DbContextOptions<ReadDbContext> options)
            : base(options)
        {
            
        }

        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            Assembly configAssembly = Assembly.GetAssembly(typeof(ReadDbContext)) ?? Assembly.GetExecutingAssembly();
            modelBuilder.ApplyConfigurationsFromAssembly(configAssembly);
        }
    }
}
