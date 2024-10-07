using FixedAssets.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FixedAssets.Application.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> GetUserByIdAsync(int userId);
        Task<bool> ProcessPurchaseAsync(int userId, int productId, int quantity);
    }
}

