using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading.Tasks;
using Xunit;
using Yocale.eShop.ApplicationCore.Entities;
using Yocale.eShop.ApplicationCore.Interfaces;
using Yocale.eShop.Infrastructure.Data;
using Yocale.eShop.WebAPI.Services;
using Yocale.eShop.WebAPI.ViewModels;

namespace Yocale.eShop.UnitTests
{
    public class CreateProduct
    {
        private readonly EShopContext eShopContext;
        private ProductRepository _mockProductRepo;
        private Mock<IAsyncRepository<Supplier>> _mockSupplierRepo;
        private Mock<IAsyncRepository<Category>> _mockCategoryRepo;
        private Mock<ILoggerFactory> _mockLogger;

        public CreateProduct()
        {
            var dbOptions = new DbContextOptionsBuilder<EShopContext>()
                .UseInMemoryDatabase("TestDb")
                .Options;

            eShopContext = new EShopContext(dbOptions);

            _mockProductRepo = new ProductRepository(eShopContext);

            _mockSupplierRepo = new Mock<IAsyncRepository<Supplier>>();

            _mockCategoryRepo = new Mock<IAsyncRepository<Category>>();

            _mockLogger = new Mock<ILoggerFactory>();
        }

        [Fact]
        public async Task Add_InvalidObjectPassed_ReturnsNull()
        {
            var supplier = new Supplier()
            {
                Name = "Supllier 1"
            };

            _mockSupplierRepo.Setup(x => x.AddAsync(supplier));

            var category = new Category()
            {
                Name = "Category 1"
            };

            _mockCategoryRepo.Setup(x => x.AddAsync(category));

            var product = new Product()
            {
                Description = "This is product 1"
            };

            var productService = new ProductService(_mockLogger.Object, _mockProductRepo, _mockSupplierRepo.Object, _mockCategoryRepo.Object);

            var data = (await productService.AddProductWithSKU(product.CategoryId, product.SupplierId, product.Name, null, product.Description)).Data;

            Assert.Null(data);
        }

        [Fact]
        public async Task Add_ValidObjectPassed_ReturnsCreatedResponse()
        {
            var supplier = new Supplier()
            {
                Name = "Supllier 1"
            };

            _mockSupplierRepo.Setup(x => x.AddAsync(supplier));

            var category = new Category()
            {
                Name = "Category 1"
            };

            _mockCategoryRepo.Setup(x => x.AddAsync(category));

            var product = new Product()
            {
                CategoryId = 1,
                SupplierId = 1,
                Name = "Product 1",
                Description = "This is product 1"
            };

            var productService = new ProductService(_mockLogger.Object, _mockProductRepo, _mockSupplierRepo.Object, _mockCategoryRepo.Object);

            var data = (await productService.AddProductWithSKU(product.CategoryId, product.SupplierId, product.Name, null, product.Description)).Data;

            Assert.IsType<ProductDetailViewModel>(data);
        }

        [Fact]
        public async Task Add_ValidObjectPassed_ReturnedResponseHasCreatedItem()
        {
            var supplier = new Supplier()
            {
                Name = "Supllier 1"
            };

            _mockSupplierRepo.Setup(x => x.AddAsync(supplier));

            var category = new Category()
            {
                Name = "Category 1"
            };

            _mockCategoryRepo.Setup(x => x.AddAsync(category));

            var product = new Product()
            {
                CategoryId = 1,
                SupplierId = 1,
                Name = "Product 1",
                Description = "This is product 1"
            };

            var productService = new ProductService(_mockLogger.Object, _mockProductRepo, _mockSupplierRepo.Object, _mockCategoryRepo.Object);

            var data = (await productService.AddProductWithSKU(product.CategoryId, product.SupplierId, product.Name, null, product.Description)).Data as ProductDetailViewModel;

            Assert.IsType<ProductDetailViewModel>(data);

            Assert.Equal("Product 1", data.Name);
        }

        [Fact]
        public async Task Add_ValidObjectPassed_ReturnedResponseHasUniqueSku()
        {

            var productService = new ProductService(_mockLogger.Object, _mockProductRepo, _mockSupplierRepo.Object, _mockCategoryRepo.Object);

            var supplier = new Supplier()
            {
                Name = "Supllier 1"
            };

            _mockSupplierRepo.Setup(x => x.AddAsync(supplier));

            var category = new Category()
            {
                Name = "Category 1"
            };

            _mockCategoryRepo.Setup(x => x.AddAsync(category));

            var product = new Product()
            {
                CategoryId = 1,
                SupplierId = 1,
                Name = "Product 1",
                Description = "This is product 1"
            };

            var data1 = (await productService.AddProductWithSKU(product.CategoryId, product.SupplierId, product.Name, null, product.Description)).Data as ProductDetailViewModel;

            product = new Product()
            {
                CategoryId = 1,
                SupplierId = 1,
                Name = "Product 2",
                Description = "This is product 2"
            };

            var data2 = (await productService.AddProductWithSKU(product.CategoryId, product.SupplierId, product.Name, null, product.Description)).Data as ProductDetailViewModel;

            Assert.IsType<ProductDetailViewModel>(data1);
            Assert.IsType<ProductDetailViewModel>(data2);
            Assert.NotEqual(data1.Sku, data2.Sku);
        }
    }
}
