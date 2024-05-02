using Core.Providers.PostgreSQL.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Providers.PostgreSQL
{
    public class PostgreSQLContext : DbContext
    {
        public DbSet<Store> Stores { get; set; }

        public PostgreSQLContext(DbContextOptions<PostgreSQLContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new StoreConfiguration());
        }
    }

    public class StoreConfiguration : IEntityTypeConfiguration<Store>
    {
        public void Configure(EntityTypeBuilder<Store> builder)
        {
            builder.ToTable("Store");
            builder.HasKey(wl => wl.Id);//create sequene for this table and set column default nextval('seq_store')
        }
    }
}
