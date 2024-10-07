using FixedAssets.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FixedAssets.Application.Interfaces
{
    public interface IOrderItemService
    {
        Task<List<OrderItemDto>> GetOrderItemsByOrderIdAsync(int orderId);
        Task AddOrderItemAsync(OrderItemDto orderItemDto);
    }
}
