using Core.Providers.PostgreSQL.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Providers.PostgreSQL
{
    public class PostgreSQLProvider : DbContext
    {
        public DbSet<Store> Stores { get; set; }

        public PostgreSQLProvider(DbContextOptions<PostgreSQLProvider> options)
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
            builder.ToTable("Stores");
            builder.HasKey(wl => wl.Id);
            //builder.Property(wl => wl.CustomerId).HasColumnType("uuid").IsRequired();
            //builder.Property(wl => wl.Name).HasColumnType("character varying")
            //                                                .HasMaxLength(200)
            //                                                .IsRequired();
            //builder.Property(wl => wl.DateCreated).HasColumnType("timestamp").IsRequired();
            //builder.Property(wl => wl.DateUpdated).HasColumnType("timestamp").IsRequired();
            //builder.Property(x => x.RecordVersion).HasColumnName("xmin")
            //                                                .HasColumnType("xid")
            //                                                .ValueGeneratedOnAddOrUpdate()
            //                                                .IsConcurrencyToken();
            //builder.HasMany(wl => wl.WorklistItems)
            //        .WithOne(e => e.WorklistEntity)
            //        .OnDelete(DeleteBehavior.Cascade);
            //builder.HasIndex(x => x.CustomerId);
        }
    }
}
