using FixedAssets.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FixedAssets.Application.Interfaces
{
    public interface IOrderService
    {
        Task<bool> ProcessOrderAsync(OrderDto orderDto);
        Task<List<OrderDto>> GetOrdersByUserIdAsync(int userId);
    }
}
