using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yocale.eShop.ApplicationCore.Entities;

namespace Yocale.eShop.Infrastructure.Data
{
    public class EShopContextSeed
    {
        public EShopContextSeed(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public static IConfiguration Configuration { get; set; }

        public static async Task SeedAsync(EShopContext eShopContext,
            ILoggerFactory loggerFactory, int? retry = 0)
        {
            int retryForAvailability = retry.Value;
            try
            {
                eShopContext.Database.Migrate();

                if (!eShopContext.Suppliers.Any())
                {
                    eShopContext.Suppliers.AddRange(
                        GetPreconfiguredSuppliers());

                    await eShopContext.SaveChangesAsync();
                }

                if (!eShopContext.Categories.Any())
                {
                    eShopContext.Categories.AddRange(
                        GetPreconfiguredCategories());

                    await eShopContext.SaveChangesAsync();
                }

                if (!eShopContext.Products.Any())
                {
                    eShopContext.Products.AddRange(
                        GetPreconfiguredProducts());

                    await eShopContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                if (retryForAvailability < 10)
                {
                    retryForAvailability++;
                    var log = loggerFactory.CreateLogger<EShopContextSeed>();
                    log.LogError(ex.Message);
                    await SeedAsync(eShopContext, loggerFactory, retryForAvailability);
                }
            }
        }

        static IEnumerable<Supplier> GetPreconfiguredSuppliers()
        {
            return new List<Supplier>()
            {
                new Supplier() { Name = "Azure"},
                new Supplier() { Name = ".NET" },
                new Supplier() { Name = "Visual Studio" },
                new Supplier() { Name = "SQL Server" },
                new Supplier() { Name = "Other" }
            };
        }

        static IEnumerable<Category> GetPreconfiguredCategories()
        {
            return new List<Category>()
            {
                new Category() { Name = "Mug"},
                new Category() { Name = "T-Shirt" },
                new Category() { Name = "Sheet" },
                new Category() { Name = "USB Memory Stick" }
            };
        }

        static IEnumerable<Product> GetPreconfiguredProducts()
        {
            return new List<Product>()
            {
                new Product() { CategoryId=2, SupplierId=2, Description = ".NET Bot Black Sweatshirt", Name = ".NET Bot Black Sweatshirt", Quantity = 10, Sku = "yp-2-2-1" },
                new Product() { CategoryId=1, SupplierId=2, Description = ".NET Black & White Mug", Name = ".NET Black & White Mug", Quantity= 1000, Sku = "yp-1-2-2" },
                new Product() { CategoryId=2, SupplierId=5, Description = "Prism White T-Shirt", Name = "Prism White T-Shirt", Quantity = 12, Sku = "yp-2-5-3" },
                new Product() { CategoryId=2, SupplierId=2, Description = ".NET Foundation Sweatshirt", Name = ".NET Foundation Sweatshirt", Quantity = 20, Sku = "yp-2-2-4" },
                new Product() { CategoryId=3, SupplierId=5, Description = "Roslyn Red Sheet", Name = "Roslyn Red Sheet", Quantity = 150, Sku = "yp-3-5-5" },
                new Product() { CategoryId=2, SupplierId=2, Description = ".NET Blue Sweatshirt", Name = ".NET Blue Sweatshirt", Quantity = 12, Sku = "yp-2-2-6" },
                new Product() { CategoryId=2, SupplierId=5, Description = "Roslyn Red T-Shirt", Name = "Roslyn Red T-Shirt", Quantity = 12, Sku = "yp-2-5-7"  },
                new Product() { CategoryId=2, SupplierId=5, Description = "Kudu Purple Sweatshirt", Name = "Kudu Purple Sweatshirt", Quantity = 20000, Sku = "yp-2-5-8" },
                new Product() { CategoryId=1, SupplierId=5, Description = "Cup<T> White Mug", Name = "Cup<T> White Mug", Quantity = 12, Sku = "yp-1-5-9" },
                new Product() { CategoryId=3, SupplierId=2, Description = ".NET Foundation Sheet", Name = ".NET Foundation Sheet", Quantity = 12, Sku = "yp-3-2-10" },
                new Product() { CategoryId=3, SupplierId=2, Description = "Cup<T> Sheet", Name = "Cup<T> Sheet", Quantity = 8500, Sku = "yp-3-2-11" },
                new Product() { CategoryId=2, SupplierId=5, Description = "Prism White TShirt", Name = "Prism White TShirt", Quantity = 12, Sku = "yp-2-5-12" }
            };
        }
    }
}
