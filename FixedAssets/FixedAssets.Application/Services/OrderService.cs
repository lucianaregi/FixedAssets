using FixedAssets.Application.DTOs;
using FixedAssets.Application.Interfaces;
using FixedAssets.Domain.Entities;
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
        private readonly IOrderRepository _orderRepository;

        public OrderService(IUserRepository userRepository, IProductRepository productRepository, IOrderRepository orderRepository)
        {
            _userRepository = userRepository;
            _productRepository = productRepository;
            _orderRepository = orderRepository;
        }

        public async Task<bool> ProcessOrder(OrderDto orderDto)
        {
            // Obter usuário e produto
            var user = await _userRepository.GetUserByIdAsync(orderDto.UserId);
            if (user == null) return false;

            // Validações de estoque e saldo para cada item do pedido
            foreach (var orderItemDto in orderDto.OrderItems)
            {
                var product = await _productRepository.GetProductByIdAsync(orderItemDto.ProductId);
                if (product == null || product.Stock < orderItemDto.Quantity || user.Balance < product.UnitPrice * orderItemDto.Quantity)
                {
                    return false;  // Se alguma validação falhar, retorna false
                }

                // Atualiza o saldo do usuário e o estoque do produto
                user.DebitBalance(product.UnitPrice * orderItemDto.Quantity);
                product.DebitStock(orderItemDto.Quantity);

                // Atualiza o banco de dados para o produto
                await _productRepository.UpdateProductAsync(product);
            }

            // Atualiza o banco de dados para o usuário
            await _userRepository.UpdateUserAsync(user);

            // Cria uma nova ordem de compra com os itens do pedido
            var newOrder = new Order
            {
                UserId = user.Id,
                OrderDate = DateTime.UtcNow,
                OrderItems = orderDto.OrderItems.Select(item => new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice
                }).ToList()
            };

            // Salva a ordem no banco de dados
            await _orderRepository.CreateOrderAsync(newOrder);

            return true;
        }



        public async Task<List<Order>> GetOrdersByUserId(int userId)
        {
            return await _orderRepository.GetOrdersByUserIdWithProduct(userId);
            
        }
    }
}
