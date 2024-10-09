using FixedAssets.Application.DTOs;
using FixedAssets.Application.Interfaces;
using FixedAssets.Infrastructure.Interfaces;
using System.Threading.Tasks;

namespace FixedAssets.Application.Services
{
    public class ToroAccountService : IToroAccountService
    {
        private readonly IToroAccountRepository _toroAccountRepository;

        public ToroAccountService(IToroAccountRepository toroAccountRepository)
        {
            _toroAccountRepository = toroAccountRepository;
        }

        public async Task<ToroAccountDto> GetAccountByUserIdAsync(int userId)
        {
            var account = await _toroAccountRepository.GetAccountByUserIdAsync(userId);
            if (account == null) return null;

            return new ToroAccountDto
            {
                UserId = account.UserId,
                AccountNumber = account.AccountNumber,
                Balance = account.Balance
            };
        }

        public async Task<bool> UpdateBalanceAsync(int userId, decimal newBalance)
        {
            var account = await _toroAccountRepository.GetAccountByUserIdAsync(userId);
            if (account == null) return false;

            account.Balance = newBalance;
            await _toroAccountRepository.UpdateAccountAsync(account);

            return true;
        }
    }
}
