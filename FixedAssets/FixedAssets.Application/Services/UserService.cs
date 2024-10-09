using FixedAssets.Application.Interfaces;
using FixedAssets.Application.DTOs;
using FixedAssets.Infrastructure.Interfaces;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using FixedAssets.Domain.Entities;

namespace FixedAssets.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IProductRepository _productRepository;
        private readonly IToroAccountService _toroAccountService; 

        public UserService(IUserRepository userRepository, IProductRepository productRepository, IToroAccountService toroAccountService)
        {
            _userRepository = userRepository;
            _productRepository = productRepository;
            _toroAccountService = toroAccountService;
        }

        // Método para buscar o usuário pelo ID
        public async Task<UserDto?> GetUserByIdAsync(int userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null) return null;

            return MapToUserDto(user);
        }

        // Atualizado: Processamento de compra usando ToroAccount
        public async Task<bool> ProcessPurchaseAsync(int userId, int productId, int quantity)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            var product = await _productRepository.GetProductByIdAsync(productId);

            if (user == null || product == null) return false;

            var totalPrice = product.UnitPrice * quantity;

            // Obtém a conta Toro do usuário
            var toroAccount = await _toroAccountService.GetAccountByUserIdAsync(userId);
            if (toroAccount == null) return false;

            // Valida se a conta Toro tem saldo suficiente
            if (toroAccount.Balance < totalPrice || !product.HasSufficientStock(quantity))
                return false;

            // Atualiza o saldo da conta Toro e o estoque do produto
            await _toroAccountService.UpdateBalanceAsync(userId, toroAccount.Balance - totalPrice);
            product.DebitStock(quantity);

            await _productRepository.UpdateProductAsync(product);

            return true;
        }

        // Método de login
        public async Task<UserDto?> LoginAsync(string email, string password)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null || !user.CheckPassword(password))
            {
                return null;
            }

            return MapToUserDto(user);
        }

        // Método para buscar usuário por e-mail
        public async Task<UserDto?> GetUserByEmailAsync(string email)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null) return null;

            return MapToUserDto(user);
        }

        // Método para mapear User para UserDto
        private UserDto MapToUserDto(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                CPF = user.CPF,
                Balance = 0, // O saldo será gerenciado pela ToroAccount
                Orders = user.Orders?.Select(o => new OrderDto
                {
                    Id = o.Id,
                    OrderDate = o.OrderDate,
                    UserId = o.UserId,
                    OrderItems = o.OrderItems?.Select(oi => new OrderItemDto
                    {
                        ProductId = oi.ProductId,
                        ProductName = oi.Product?.Name,
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
    }
}
