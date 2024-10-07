using FixedAssets.Application.DTOs;
using FixedAssets.Application.Interfaces;
using FixedAssets.Domain.Entities;
using FixedAssets.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<bool> ProcessOrderAsync(OrderDto orderDto)
        {
            
            var user = await _userRepository.GetUserByIdAsync(orderDto.UserId);
            if (user == null) return false;

            
            foreach (var orderItemDto in orderDto.OrderItems)
            {
                var product = await _productRepository.GetProductByIdAsync(orderItemDto.ProductId);
                if (product == null || !product.HasSufficientStock(orderItemDto.Quantity) || !user.HasSufficientBalance(product.UnitPrice * orderItemDto.Quantity))
                {
                    return false; 
                }

                
                user.DebitBalance(product.UnitPrice * orderItemDto.Quantity);
                product.DebitStock(orderItemDto.Quantity);

                
                await _productRepository.UpdateProductAsync(product);
            }

           
            await _userRepository.UpdateUserAsync(user);

            
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

            
            await _orderRepository.CreateOrderAsync(newOrder);

            return true;
        }

        public async Task<List<OrderDto>> GetOrdersByUserIdAsync(int userId)
        {
            var orders = await _orderRepository.GetOrdersByUserIdWithProduct(userId);

            
            return orders.Select(order => new OrderDto
            {
                Id = order.Id,
                UserId = order.UserId,
                OrderDate = order.OrderDate,
                OrderItems = order.OrderItems.Select(item => new OrderItemDto
                {
                    ProductId = item.ProductId,
                    ProductName = item.Product != null ? item.Product.Name : "Produto não encontrado",
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice
                }).ToList()
            }).ToList();
        }

    }
}
