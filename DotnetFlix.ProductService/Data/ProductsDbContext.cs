using Microsoft.EntityFrameworkCore;
using DotnetFlix.ProductService.Models;


namespace DotnetFlix.ProductService.Data
{
    public class ProductsDbContext : DbContext
    {

        public ProductsDbContext()
        {
            //...
        }

        public ProductsDbContext(DbContextOptions<ProductsDbContext> options) : base(options)
        {
            // ...
        }

        public DbSet<Product> Product { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Set integrity
            builder.Entity<Product>(entity =>
            {
                entity.Property(x => x.Name).HasMaxLength(50).IsRequired();
                entity.Property(x => x.Description).HasMaxLength(200).IsRequired();
                entity.Property(x => x.Details).IsRequired();
                entity.Property(x => x.Photo).HasMaxLength(200).IsRequired();
                entity.Property(x => x.Price).IsRequired().HasColumnType("decimal(18,2)");
                entity.Property(x => x.Quantity).IsRequired().HasDefaultValue(0);
            });

            // Set index on product name for faster searching
            builder.Entity<Product>().HasIndex(x => x.Name);
        }
    }
}
