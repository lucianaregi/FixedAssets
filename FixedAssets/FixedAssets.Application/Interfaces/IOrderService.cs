using FixedAssets.Application.DTOs;
using FixedAssets.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FixedAssets.Application.Interfaces
{
    public interface IOrderService
    {
        Task<bool> ProcessOrder(OrderDto order);
        Task<List<Order>> GetOrdersByUserId(int userId);
    }
}
