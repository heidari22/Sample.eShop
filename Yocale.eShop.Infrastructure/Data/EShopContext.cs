using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Yocale.eShop.ApplicationCore.Entities;
using Yocale.eShop.ApplicationCore.Entities.BasketAggregate;
using Yocale.eShop.ApplicationCore.Entities.OrderAggregate;

namespace Yocale.eShop.Infrastructure.Data
{
    public class EShopContext : DbContext
    {
        public EShopContext(DbContextOptions<EShopContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Basket> Baskets { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Product>(ConfigureProduct);
            builder.Entity<Category>(ConfigureCategory);
            builder.Entity<Supplier>(ConfigureSupplier);
            builder.Entity<Basket>(ConfigureBasket);
            builder.Entity<Order>(ConfigureOrder);
            builder.Entity<OrderItem>(ConfigureOrderItem);
            builder.Entity<Address>(ConfigureAddress);
            builder.Entity<ProductItemOrdered>(ConfigurateProductItemOrdered);
        }

        private void ConfigureSupplier(EntityTypeBuilder<Supplier> builder)
        {
            builder.ToTable("Supplier");

            builder.HasKey(s => s.Id);

            builder.Property(s => s.Id)
               .ForSqlServerUseSequenceHiLo("supplier_hilo")
               .IsRequired();

            builder.Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(100);
        }

        private void ConfigureCategory(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Category");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
               .ForSqlServerUseSequenceHiLo("category_hilo")
               .IsRequired();

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);
        }
        
        private void ConfigureProduct(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Product");

            builder.Property(p => p.Id)
                .ForSqlServerUseSequenceHiLo("product_hilo")
                .IsRequired();

            builder.Property(p => p.Name)
               .HasMaxLength(50)
               .IsRequired();

            builder.Property(p => p.Sku)
                .HasMaxLength(50)
                .IsRequired();

            builder.HasIndex(p => p.Sku)
                .IsUnique();

            builder.HasOne(ci => ci.Category)
                 .WithMany()
                 .HasForeignKey(ci => ci.CategoryId);

            builder.HasOne(ci => ci.Supplier)
                .WithMany()
                .HasForeignKey(ci => ci.SupplierId);

        }

        private void ConfigureBasket(EntityTypeBuilder<Basket> builder)
        {
            var navigation = builder.Metadata.FindNavigation(nameof(Basket.Items));

            navigation.SetPropertyAccessMode(PropertyAccessMode.Field);
        }

        private void ConfigureOrder(EntityTypeBuilder<Order> builder)
        {
            var navigation = builder.Metadata.FindNavigation(nameof(Order.OrderItems));

            navigation.SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.OwnsOne(o => o.ShipToAddress);
        }

        private void ConfigureOrderItem(EntityTypeBuilder<OrderItem> builder)
        {
            builder.OwnsOne(i => i.ItemOrdered);
        }

        private void ConfigureAddress(EntityTypeBuilder<Address> builder)
        {
            builder.Property(a => a.ZipCode)
                .HasMaxLength(18)
                .IsRequired();

            builder.Property(a => a.Street)
                .HasMaxLength(180)
                .IsRequired();

            builder.Property(a => a.State)
                .HasMaxLength(60);

            builder.Property(a => a.Country)
                .HasMaxLength(90)
                .IsRequired();

            builder.Property(a => a.City)
                .HasMaxLength(100)
                .IsRequired();
        }

        private void ConfigurateProductItemOrdered(EntityTypeBuilder<ProductItemOrdered> builder)
        {
            builder.Property(pio => pio.ProductName)
                .HasMaxLength(50)
                .IsRequired();
        }

    }
}
