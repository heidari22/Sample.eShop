using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yocale.eShop.Resource.Errors;
using Yocale.eShop.Utility.Data;
using Yocale.eShop.WebAPI.Interfaces;
using Yocale.eShop.WebAPI.ViewModels;

namespace Yocale.eShop.WebAPI.Services
{
    public class CachedProductService : ICachedProductService
    {
        private readonly IMemoryCache _cache;
        private readonly ProductService _productService;
        private static readonly string _suppliersKey = "suppliers";
        private static readonly string _categoriesKey = "categories";
        private static readonly string _productsKey = "products-{0}-{1}-{2}-{3}";
        private static readonly string _productKey = "product-{0}";

        private static readonly TimeSpan _defaultCacheDuration = TimeSpan.FromSeconds(30);

        public CachedProductService(IMemoryCache cache,
            ProductService productService)
        {
            _cache = cache;
            _productService = productService;
        }

        public async Task<ResultModel<ProductIndexViewModel>> GetProductList(int pageIndex, int itemsPage, int? supplierId, int? categoryId)
        {
            try
            {
                string cacheKey = String.Format(_productsKey, pageIndex, itemsPage, supplierId, categoryId);
                return await _cache.GetOrCreateAsync(cacheKey, async entry =>
                {
                    entry.SlidingExpiration = _defaultCacheDuration;
                    return await _productService.GetProductList(pageIndex, itemsPage, supplierId, categoryId);
                });
            }
            catch
            {
                return ResultModel<ProductIndexViewModel>.Create(new InternalServerError());
            }
           
        }

        public async Task<ResultModel<List<SelectListModel>>> GetSuppliers()
        {
            return await _cache.GetOrCreateAsync(_suppliersKey, async entry =>
            {
                entry.SlidingExpiration = _defaultCacheDuration;
                return await _productService.GetSuppliers();
            });
        }

        public async Task<ResultModel<List<SelectListModel>>> GetCategories()
        {
            return await _cache.GetOrCreateAsync(_categoriesKey, async entry =>
            {
                entry.SlidingExpiration = _defaultCacheDuration;
                return await _productService.GetCategories();
            });
        }

        public async Task<ResultModel<ProductDetailViewModel>> GetById(int id)
        {
            try
            {
                string cacheKey = String.Format(_productKey, id);
                return await _cache.GetOrCreateAsync(cacheKey, async entry =>
                {
                    entry.SlidingExpiration = _defaultCacheDuration;
                    return await _productService.GetById(id);
                });
            }
            catch
            {
                return ResultModel<ProductDetailViewModel>.Create(new InternalServerError());
            }
         
        }
    }
}
