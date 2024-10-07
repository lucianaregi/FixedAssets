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
        Task<List<Product>> GetAllProductsAsync(); 
        Task<Product> GetProductByIdAsync(int id); 
        Task UpdateProductAsync(Product product);  
    }
}

