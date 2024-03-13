using DataCore.Providers.MSSQL.Entity;
using Microsoft.EntityFrameworkCore;

namespace DataCore.Providers.MSSQL
{
    public class MSSQLProvider : DbContext
    {
        public MSSQLProvider() { }
        public MSSQLProvider(DbContextOptions<MSSQLProvider> options):base(options) { }

        public virtual DbSet<Store> Order { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
