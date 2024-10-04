using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FixedAssets.Application.Interfaces
{
    public interface IProductService
    {
        Task<List<ProductDto>> GetProductsAsync(); // Retorna a lista de produtos de renda fixa
        Task<ProductDto> GetProductByIdAsync(int productId); // Obtém detalhes de um produto específico
        Task<bool> ValidateStockAsync(int productId, int quantity); // Valida se há estoque suficiente para uma compra
    }
}
