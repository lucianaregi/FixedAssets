using FixedAssets.Application.DTOs;
using FixedAssets.Application.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FixedAssets.Application.Interfaces
{
    public interface IOrderService
    {
        Task<OrderProcessingResult> ProcessOrderAsync(OrderDto orderDto);
        Task<List<OrderDto>> GetOrdersByUserIdAsync(int userId);
    }
}
