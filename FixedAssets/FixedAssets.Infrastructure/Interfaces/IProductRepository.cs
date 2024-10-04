using FixedAssets.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FixedAssets.Infrastructure.Interfaces
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllProductsAsync(); // Retorna todos os produtos
        Task<Product> GetProductByIdAsync(int id); // Retorna um produto específico por ID
        Task UpdateProductAsync(Product product);  // Atualiza o estoque do produto após a compra
    }
}

