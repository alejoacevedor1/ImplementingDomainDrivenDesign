using System.Configuration;
using Domain.Entities;
using Domain.Entities.Product;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    /// <summary>
    /// Class DbContext
    /// </summary>
    public class InventoryContext : DbContext
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="options"></param>
        public InventoryContext(DbContextOptions<InventoryContext> options) : base(options)
        {
        }

        /// <summary>
        /// Dbset products
        /// </summary>
        public DbSet<Product> Products { get; set; }

        /// <summary>
        /// Dbset providers
        /// </summary>
        public DbSet<Provider> Providers { get; set; }

        /// <summary>
        /// Override OnModelCreating.
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(product =>
            {
                product.ToTable("Product");
                product.HasKey(p => p.IdProduct);
                product.Property(p => p.Description).IsRequired().HasMaxLength(300);
                product.Property(p => p.Active).IsRequired();
                product.Property(p => p.ManufacturingDate);
                product.Property(p => p.ValidityDate);
                product.Property(p => p.IdProvider).IsRequired();
            });

            modelBuilder.Entity<Provider>(provider =>
            {
                provider.ToTable("Provider");
                provider.HasKey(p => p.IdProvider);
                provider.Property(p => p.Description).HasMaxLength(300);
                provider.Property(p => p.Phone).HasMaxLength(20);
            });
        }
    }
}

