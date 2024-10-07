using FixedAssets.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FixedAssets.Infrastructure.Interfaces
{
    public interface IOrderItemRepository
    {
        Task<List<OrderItem>> GetOrderItemsByOrderId(int orderId);
        Task AddOrderItem(OrderItem orderItem);
    }
}
