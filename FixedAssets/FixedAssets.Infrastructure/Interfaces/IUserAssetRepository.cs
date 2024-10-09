using FixedAssets.Domain.Entities;
using System.Threading.Tasks;

namespace FixedAssets.Infrastructure.Interfaces
{
    public interface IUserAssetRepository
    {
        Task<UserAsset?> GetByUserIdAndProductIdAsync(int userId, int productId);
        Task AddAsync(UserAsset userAsset);
        Task UpdateAsync(UserAsset userAsset);
    }
}
