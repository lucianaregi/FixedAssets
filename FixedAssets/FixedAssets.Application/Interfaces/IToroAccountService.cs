using FixedAssets.Application.DTOs;
using System.Threading.Tasks;

namespace FixedAssets.Application.Interfaces
{
    public interface IToroAccountService
    {
        Task<ToroAccountDto> GetAccountByUserIdAsync(int userId);
        Task<bool> UpdateBalanceAsync(int userId, decimal newBalance);
    }
}
