using FixedAssets.Application.Interfaces;
using FixedAssets.Application.DTOs;
using FixedAssets.Infrastructure.Interfaces;

namespace FixedAssets.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IProductRepository _productRepository;

        public UserService(IUserRepository userRepository, IProductRepository productRepository)
        {
            _userRepository = userRepository;
            _productRepository = productRepository;
        }

        public async Task<UserDto> GetUserByIdAsync(int userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null) return null;

            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Balance = user.Balance,
                Assets = user.Assets.Select(a => new UserAssetDto
                {
                    ProductId = a.ProductId,
                    ProductName = a.ProductName,
                    Quantity = a.Quantity
                }).ToList()
            };
        }

        public async Task<bool> ProcessPurchaseAsync(int userId, int productId, int quantity)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            var product = await _productRepository.GetProductByIdAsync(productId);

            if (user == null || product == null) return false;

            var totalPrice = product.UnitPrice * quantity;

            if (!user.HasSufficientBalance(totalPrice) || !product.HasSufficientStock(quantity))
                return false;

            user.DebitBalance(totalPrice);
            product.DebitStock(quantity);

            // Atualiza o usuário e o produto após a compra
            await _userRepository.UpdateUserAsync(user);
            await _productRepository.UpdateProductAsync(product);

            return true;
        }
    }
}
