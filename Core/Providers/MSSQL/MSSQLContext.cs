using Core.Providers.MSSQL.Entity;
using Microsoft.EntityFrameworkCore;

namespace Core.Providers.MSSQL
{
    public class MSSQLContext : DbContext
    {
        public MSSQLContext() { }
        public MSSQLContext(DbContextOptions<MSSQLContext> options) : base(options) { }

        public virtual DbSet<Store> Store { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
