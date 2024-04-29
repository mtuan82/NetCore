using Core.Providers.MySQL.Entity;
using Microsoft.EntityFrameworkCore;

namespace Core.Providers.MySQL
{
    public class MySQLProvider : DbContext
    {
        public MySQLProvider() { }
        public MySQLProvider(DbContextOptions<MySQLProvider> options) : base(options) { }

        public DbSet<Customer> Book { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired();
            });
        }
    }
}
