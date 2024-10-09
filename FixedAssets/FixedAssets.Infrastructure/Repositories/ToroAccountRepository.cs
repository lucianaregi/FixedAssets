using FixedAssets.Domain.Entities;
using FixedAssets.Infrastructure.Interfaces;
using FixedAssets.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace FixedAssets.Infrastructure.Repositories
{
    public class ToroAccountRepository : IToroAccountRepository
    {
        private readonly ApplicationDbContext _context;

        public ToroAccountRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Busca a conta Toro pelo ID do usuário
        public async Task<ToroAccount?> GetAccountByUserIdAsync(int userId)
        {
            return await _context.ToroAccounts.FirstOrDefaultAsync(a => a.UserId == userId);
        }

        // Atualiza a conta Toro
        public async Task UpdateAccountAsync(ToroAccount account)
        {
            _context.ToroAccounts.Update(account);
            await _context.SaveChangesAsync();
        }
    }
}
