using FixedAssets.Application.Interfaces;
using FixedAssets.Application.DTOs;
using FixedAssets.Domain.Entities;
using FixedAssets.Infrastructure.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FixedAssets.Application.Services
{
    public class OrderItemService : IOrderItemService
    {
        private readonly IOrderItemRepository _orderItemRepository;

        public OrderItemService(IOrderItemRepository orderItemRepository)
        {
            _orderItemRepository = orderItemRepository;
        }

       
        public async Task<List<OrderItemDto>> GetOrderItemsByOrderIdAsync(int orderId)
        {
            var orderItems = await _orderItemRepository.GetOrderItemsByOrderId(orderId);

            
            var orderItemDtos = orderItems?.ConvertAll(item => new OrderItemDto
            {
                ProductId = item.ProductId,
                ProductName = item.Product?.Name,  
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice
            });

            return orderItemDtos ?? new List<OrderItemDto>();
        }

        
        public async Task AddOrderItemAsync(OrderItemDto orderItemDto)
        {
            
            var orderItem = new OrderItem
            {
                ProductId = orderItemDto.ProductId,
                Quantity = orderItemDto.Quantity,
                UnitPrice = orderItemDto.UnitPrice
                
            };

            await _orderItemRepository.AddOrderItemAsync(orderItem);
        }
    }
}
