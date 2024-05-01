using Core.Providers.MySQL.Entity;
using Microsoft.EntityFrameworkCore;
using MySql.EntityFrameworkCore.Extensions;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace Core.Providers.MySQL
{
    public class MySQLContext : DbContext
    {
        public MySQLContext() { }
        public MySQLContext(DbContextOptions<MySQLContext> options) : base(options) { }

        public DbSet<Customer> Customer { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Customer>(entity =>
            {
                //alter table provider.customer modify column Id int auto_increment
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ForMySQLHasDefaultValue(0);
                entity.Property(e => e.Id).UseMySQLAutoIncrementColumn("int");
                entity.Property(e => e.Name).IsRequired();
            });
        }
    }
}
