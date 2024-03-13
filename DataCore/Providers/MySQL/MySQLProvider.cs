using DataCore.Providers.MySQL.Entity;
using Microsoft.EntityFrameworkCore;

namespace DataCore.Providers.MySQL
{
    public class MySQLProvider: DbContext
    {
        public MySQLProvider() { }

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
