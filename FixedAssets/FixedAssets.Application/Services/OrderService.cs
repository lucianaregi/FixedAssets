using FixedAssets.Application.DTOs;
using FixedAssets.Application.Interfaces;
using FixedAssets.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FixedAssets.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUserRepository _userRepository;
        private readonly IProductRepository _productRepository;

        public OrderService(IUserRepository userRepository, IProductRepository productRepository)
        {
            _userRepository = userRepository;
            _productRepository = productRepository;
        }

        public async Task<bool> ProcessOrder(OrderDto order)
        {
            // Obter usuário e produto
            var user = await _userRepository.GetUserByIdAsync(order.UserId);
            var product = await _productRepository.GetProductByIdAsync(order.ProductId);

            if (user == null || product == null || product.Stock < order.Quantity || user.Balance < product.UnitPrice * order.Quantity)
                return false;

            // Atualizar saldo e estoque
            user.Balance -= product.UnitPrice * order.Quantity;
            product.Stock -= order.Quantity;

            // Atualizar o banco de dados
            await _userRepository.UpdateUserAsync(user);
            await _productRepository.UpdateProductAsync(product);

            return true;
        }
    }

}
