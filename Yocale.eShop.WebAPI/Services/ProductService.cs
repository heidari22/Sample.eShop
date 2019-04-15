using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yocale.eShop.ApplicationCore.Entities;
using Yocale.eShop.ApplicationCore.Interfaces;
using Yocale.eShop.ApplicationCore.Specifications;
using Yocale.eShop.Resource.Errors;
using Yocale.eShop.Utility.Data;
using Yocale.eShop.Utility.Resource;
using Yocale.eShop.WebAPI.Interfaces;
using Yocale.eShop.WebAPI.ViewModels;

namespace Yocale.eShop.WebAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly ILogger<ProductService> _logger;
        private readonly IProductRepository _productRepository;
        private readonly IAsyncRepository<Supplier> _supplierRepository;
        private readonly IAsyncRepository<Category> _categoryRepository;
      
        public ProductService(
            ILoggerFactory loggerFactory,
            IProductRepository productRepository,
            IAsyncRepository<Supplier> supplierRepository,
            IAsyncRepository<Category> categoryRepository)
        {
            _logger = loggerFactory.CreateLogger<ProductService>();
            _productRepository = productRepository;
            _supplierRepository = supplierRepository;
            _categoryRepository = categoryRepository; 
        }

        public async Task<ResultModel<ProductDetailViewModel>> AddProductWithSKU(int categoryId, int supplierId, string name, int? quantity, string description)
        {
            try
            {
                if (string.IsNullOrEmpty(name) || categoryId <= 0 || supplierId <= 0)
                    return ResultModel<ProductDetailViewModel>.Create(new BadRequestError() { Message = Shared.InvalidProduct });

                Product product = new Product()
                {
                    CategoryId = categoryId,
                    SupplierId = supplierId,
                    Name = name,
                    Quantity = quantity,
                    Description = description,
                };

                product = await _productRepository.CreateProductWithSKUAsync(product);

                return new ProductDetailViewModel()
                {
                    Id = product.Id,
                    Name = product.Name,
                    Sku = product.Sku,
                    Description = product.Description,
                    Quantity = product.Quantity,
                    CategoryId = product.CategoryId,
                    SupplierId = product.SupplierId
                }.ToResultModel();
            }
            catch
            {
                return ResultModel<ProductDetailViewModel>.Create(new InternalServerError());
            }
        }
           
        public async Task<ResultModel<ProductDetailViewModel>> GetById(int id)
        {
            _logger.LogInformation("GetProductById called.");

            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
                return ResultModel<ProductDetailViewModel>.Create(new NotFoundError()); 

            var vm = new ProductDetailViewModel()
            {
                Id = product.Id,
                Name = product.Name,
                Sku = product.Sku,
                Quantity = product.Quantity,
                Description = product.Description,
                CategoryId = product.CategoryId,
                SupplierId = product.SupplierId
            };

            return vm.ToResultModel();
        }

        public async Task<ResultModel<List<SelectListModel>>> GetCategories()
        {
            _logger.LogInformation("GetCategories called.");

            var categories = await _categoryRepository.ListAllAsync();

            var items = new List<SelectListModel>();

            foreach (Category category in categories)
            {
                var ctg = new SelectListModel { Id = category.Id, Name = category.Name };

                items.Add(ctg);
            }

            return items.ToResultModel();
        }

        public async Task<ResultModel<ProductIndexViewModel>> GetProductList(int pageIndex, int itemsPage, int? supplierId, int? categoryId)
        {
            _logger.LogInformation("GetProductList called.");

            var filterSpecification = new ProductFilterSpecification(supplierId, categoryId);

            var filterPaginatedSpecification =
                new ProductFilterPaginatedSpecification(itemsPage * pageIndex, itemsPage, supplierId, categoryId);

            var itemsOnPage = await _productRepository.ListAsync(filterPaginatedSpecification);

            var totalItems = await _productRepository.CountAsync(filterSpecification);

            var vm = new ProductIndexViewModel()
            {
                ProductList = itemsOnPage.Select(i => new ProductDetailViewModel()
                {
                    Id = i.Id,
                    Name = i.Name,
                    Sku = i.Sku,
                    Quantity = i.Quantity,
                    Description = i.Description,
                    CategoryId = i.CategoryId,
                    SupplierId = i.SupplierId
                }).ToPagedList(totalItems, int.Parse(Math.Ceiling(((decimal)totalItems / itemsPage)).ToString()), itemsPage, ++pageIndex),

                Suppliers = (await GetSuppliers()).Data,

                Categories = (await GetCategories()).Data,

                SupplierFilterApplied = supplierId,

                CategoryFilterApplied = categoryId
            };

            return vm.ToResultModel();
        }

        public async Task<ResultModel<List<SelectListModel>>> GetSuppliers()
        {
            _logger.LogInformation("GetSuppliers called.");

            var suppliers = await _supplierRepository.ListAllAsync();

            var items = new List<SelectListModel>();
        
            foreach (Supplier supplier in suppliers)
            {
                var splr = new SelectListModel { Id = supplier.Id, Name = supplier.Name };

                items.Add(splr);
            }

            return items.ToResultModel();
        }

        public async Task<ResultModel<bool>> UpdateProduct(int productId, int categoryId, int supplierId, string name, int? quantity, string description)
        {
            try
            {
                if (categoryId <= 0 || supplierId <= 0 || string.IsNullOrEmpty(name))
                    return ResultModel<bool>.Create(new BadRequestError() { Message = Shared.InvalidProduct });

                var product = await _productRepository.GetByIdAsync(productId);

                if (product == null)
                    return ResultModel<bool>.Create(new NotFoundError());

                product.CategoryId = categoryId;
                product.SupplierId = supplierId;
                product.Name = name;
                product.Quantity = quantity;
                product.Description = description;
                product.Sku = await _productRepository.GenerateSKUAsync(productId, categoryId, supplierId);
            
                await _productRepository.UpdateAsync(product);

                return true.ToResultModel();
            }
            catch (Exception ex)
            {
                if (ex.GetType().FullName ==
                         "Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException")
                {
                    return ResultModel<bool>.Create(new NotFoundError());
                }

                return ResultModel<bool>.Create(new InternalServerError());
            }
        }

        public async Task<ResultModel<bool>> DeleteProduct(int id)
        {
            try
            {
                if (id <= 0)
                    return ResultModel<bool>.Create(new BadRequestError());

                var product = await _productRepository.GetByIdAsync(id);

                if (product == null)
                    return ResultModel<bool>.Create(new NotFoundError());

                await _productRepository.DeleteAsync(product);

                return true.ToResultModel();
            }
            catch (Exception ex)
            {
                if (ex.GetType().FullName ==
                         "Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException")
                {
                    return ResultModel<bool>.Create(new NotFoundError());
                }

                return ResultModel<bool>.Create(new InternalServerError());
            }
        }

    }
}
