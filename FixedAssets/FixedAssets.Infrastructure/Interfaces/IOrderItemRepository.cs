using FixedAssets.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FixedAssets.Infrastructure.Interfaces
{
    public interface IOrderItemRepository : IGenericRepository<OrderItem>
    {
        Task<List<OrderItem>> GetOrderItemsByOrderId(int orderId);
        Task AddOrderItemAsync(OrderItem orderItem);
    }
}
