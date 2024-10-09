using FixedAssets.Domain.Entities;
using System.Threading.Tasks;

namespace FixedAssets.Infrastructure.Interfaces
{
    public interface IToroAccountRepository 
    {
        Task<ToroAccount?> GetAccountByUserIdAsync(int userId);
        Task UpdateAccountAsync(ToroAccount account);
    }
}
