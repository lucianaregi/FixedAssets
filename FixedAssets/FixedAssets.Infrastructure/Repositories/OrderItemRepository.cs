using FixedAssets.Domain.Entities;
using FixedAssets.Infrastructure.Interfaces;
using FixedAssets.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FixedAssets.Infrastructure.Repositories
{
    public class OrderItemRepository : GenericRepository<OrderItem>, IOrderItemRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderItemRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

       
        public async Task<List<OrderItem>> GetOrderItemsByOrderId(int orderId)
        {
            return await _context.OrderItems
                .Where(oi => oi.OrderId == orderId)
                .Include(oi => oi.Product)
                .ToListAsync();
        }

        
        public async Task AddOrderItemAsync(OrderItem orderItem)
        {
            await _context.OrderItems.AddAsync(orderItem);
            await _context.SaveChangesAsync();
        }

        
        public async Task UpdateOrderItemAsync(OrderItem orderItem)
        {
            _context.OrderItems.Update(orderItem);
            await _context.SaveChangesAsync();
        }

        
        public async Task DeleteOrderItemAsync(int orderItemId)
        {
            var orderItem = await _context.OrderItems.FindAsync(orderItemId);
            if (orderItem != null)
            {
                _context.OrderItems.Remove(orderItem);
                await _context.SaveChangesAsync();
            }
        }

        
        public async Task<OrderItem?> GetOrderItemByIdAsync(int orderItemId)
        {
            return await _context.OrderItems
                .Include(oi => oi.Product)
                .FirstOrDefaultAsync(oi => oi.OrderId == orderItemId);
        }
    }
}
