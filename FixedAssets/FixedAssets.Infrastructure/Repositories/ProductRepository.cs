using System.Reflection;
using System.Text.Json;
using FixedAssets.Domain.Entities;
using FixedAssets.Infrastructure.Interfaces;

namespace FixedAssets.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly string _filePath = "..\\FixedAssets.Infrastructure\\Data\\products.json";


        public async Task<List<Product>> GetAllProductsAsync()
        {
            if (!File.Exists(_filePath)) return new List<Product>();

            var jsonData = await File.ReadAllTextAsync(_filePath);
            return JsonSerializer.Deserialize<List<Product>>(jsonData);
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            var products = await GetAllProductsAsync();
            return products.FirstOrDefault(p => p.Id == id);
        }

        public async Task UpdateProductAsync(Product product)
        {
            var products = await GetAllProductsAsync();
            var productToUpdate = products.FirstOrDefault(p => p.Id == product.Id);
            if (productToUpdate != null)
            {
                productToUpdate.Stock = product.Stock;
                var jsonData = JsonSerializer.Serialize(products);
                await File.WriteAllTextAsync(_filePath, jsonData);
            }
        }
    }
}
