using FixedAssets.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FixedAssets.Application.UseCases
{
    public class ProcessPurchaseUseCase
    {
        private readonly IUserService _userService;
        private readonly IProductService _productService;

        public ProcessPurchaseUseCase(IUserService userService, IProductService productService)
        {
            _userService = userService;
            _productService = productService;
        }

        public async Task<bool> ExecuteAsync(int userId, int productId, int quantity)
        {
            // Verifica se o produto tem estoque suficiente
            var product = await _productService.GetProductByIdAsync(productId);
            if (product == null || !await _productService.ValidateStockAsync(productId, quantity))
            {
                throw new InvalidOperationException("Estoque insuficiente.");
            }

            // Processa a compra pelo usuário
            return await _userService.ProcessPurchaseAsync(userId, productId, quantity);
        }
    }
}
