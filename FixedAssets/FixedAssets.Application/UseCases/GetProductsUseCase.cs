using FixedAssets.Application.DTOs;
using FixedAssets.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FixedAssets.Application.UseCases
{
    public class GetProductsUseCase
    {
        private readonly IProductService _productService;

        public GetProductsUseCase(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<List<ProductDto>> ExecuteAsync()
        {
            var products = await _productService.GetProductsAsync();
            // Ordenar os produtos pela taxa de retorno (taxa decrescente)
            return products.OrderByDescending(p => p.Tax).ToList();
        }
    }
}
