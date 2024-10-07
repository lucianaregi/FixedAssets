using FixedAssets.Application.Interfaces;
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

        public async Task<List<OrderItem>> GetOrderItemsByOrderId(int orderId)
        {
            return await _orderItemRepository.GetOrderItemsByOrderId(orderId);
        }

        public async Task AddOrderItem(OrderItem orderItem)
        {
            await _orderItemRepository.AddOrderItem(orderItem);
        }
    }
}
