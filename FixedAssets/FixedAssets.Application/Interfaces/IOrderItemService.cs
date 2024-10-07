using FixedAssets.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FixedAssets.Application.Interfaces
{
    public interface IOrderItemService
    {
        Task<List<OrderItem>> GetOrderItemsByOrderId(int orderId);
        Task AddOrderItem(OrderItem orderItem);
    }
}
