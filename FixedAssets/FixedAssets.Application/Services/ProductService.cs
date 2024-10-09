using FixedAssets.Application.Interfaces;
using FixedAssets.Application.DTOs;
using FixedAssets.Infrastructure.Interfaces;

namespace FixedAssets.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<List<ProductDto>> GetProductsAsync()
        {
            var products = await _productRepository.GetAllProductsAsync();
            return products.OrderByDescending(p => p.Tax)
                .Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Indexer = p.Indexer,
                Tax = p.Tax,
                UnitPrice = p.UnitPrice,
                Stock = p.Stock
            }).ToList();
        }

        public async Task<ProductDto> GetProductByIdAsync(int productId)
        {
            var product = await _productRepository.GetProductByIdAsync(productId);
            if (product == null) return null;

            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Indexer = product.Indexer,
                Tax = product.Tax,
                UnitPrice = product.UnitPrice,
                Stock = product.Stock
            };
        }

        public async Task<bool> ValidateStockAsync(int productId, int quantity)
        {
            var product = await _productRepository.GetProductByIdAsync(productId);
            return product != null && product.HasSufficientStock(quantity);
        }
    }
}
