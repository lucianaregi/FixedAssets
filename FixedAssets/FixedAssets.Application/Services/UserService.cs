using FixedAssets.Application.Interfaces;
using FixedAssets.Application.DTOs;
using FixedAssets.Infrastructure.Interfaces;
using FixedAssets.Domain.Entities;

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
                CPF = user.CPF,
                Balance = user.Balance,
                Orders = user.Orders?.Select(o => new OrderDto
                {
                    Id = o.Id,
                    OrderDate = o.OrderDate,
                    UserId = o.UserId,
                    OrderItems = o.OrderItems?.Select(oi => new OrderItemDto
                    {
                        ProductId = oi.ProductId,
                        ProductName = oi.Product.Name,  
                        Quantity = oi.Quantity,
                        UnitPrice = oi.UnitPrice
                    }).ToList()
                }).ToList(),
                Assets = user.Assets?.Select(a => new UserAssetDto
                {
                    ProductId = a.ProductId,
                    ProductName = a.ProductName,
                    Quantity = a.Quantity
                }).ToList() ?? new List<UserAssetDto>()
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
