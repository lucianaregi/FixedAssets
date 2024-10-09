using FixedAssets.Domain.Entities;
using FixedAssets.Infrastructure.Interfaces;
using FixedAssets.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace FixedAssets.Infrastructure.Repositories
{
    public class UserAssetRepository : IUserAssetRepository
    {
        private readonly ApplicationDbContext _context;

        public UserAssetRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        
        public async Task<UserAsset?> GetByUserIdAndProductIdAsync(int userId, int productId)
        {
            return await _context.UserAssets
                .FirstOrDefaultAsync(ua => ua.UserId == userId && ua.ProductId == productId);
        }

        
        public async Task AddAsync(UserAsset userAsset)
        {
            await _context.UserAssets.AddAsync(userAsset);
            await _context.SaveChangesAsync();
        }

        
        public async Task UpdateAsync(UserAsset userAsset)
        {
            _context.UserAssets.Update(userAsset);
            await _context.SaveChangesAsync();
        }
    }
}
