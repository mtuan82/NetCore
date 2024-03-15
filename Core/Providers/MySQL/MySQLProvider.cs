using Core.Providers.MySQL.Entity;
using Microsoft.EntityFrameworkCore;

namespace Core.Providers.MySQL
{
    public class MySQLProvider : DbContext
    {
        public MySQLProvider() { }
        public MySQLProvider(DbContextOptions<MySQLProvider> options) : base(options) { }

        public DbSet<Book> Book { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Book>(entity =>
            {
                entity.HasKey(e => e.ISBN);
                entity.Property(e => e.Title).IsRequired();
            });
        }
    }
}
