using FixedAssets.Application.DTOs;
using FixedAssets.Application.Interfaces;
using FixedAssets.Application.Responses;
using FixedAssets.Domain.Entities;
using FixedAssets.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FixedAssets.Application.Services
{

    public class OrderService : IOrderService
    {
        private readonly IUserRepository _userRepository;
        private readonly IProductRepository _productRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IUserAssetRepository _userAssetRepository;

        public OrderService(IUserRepository userRepository, IProductRepository productRepository, IOrderRepository orderRepository, IUserAssetRepository userAssetRepository)
        {
            _userRepository = userRepository;
            _productRepository = productRepository;
            _orderRepository = orderRepository;
            _userAssetRepository = userAssetRepository;
        }

        public async Task<OrderProcessingResult> ProcessOrderAsync(OrderDto orderDto)
        {
            var result = new OrderProcessingResult();

            try
            {
                // Valida o usuário
                var user = await GetUserAsync(orderDto.UserId, result);
                if (!result.Success) return result;

                // Processa cada item do pedido
                foreach (var orderItemDto in orderDto.OrderItems)
                {
                    // Valida o produto
                    var product = await GetProductAsync(orderItemDto.ProductId, result);
                    if (!result.Success) return result;

                    // Valida saldo e estoque do produto
                    if (!ValidateProductAndUser(user.ToroAccount, product, orderItemDto, result))
                        return result;
                }

                // Tudo validado, agora podemos debitar o saldo e atualizar o estoque
                foreach (var orderItemDto in orderDto.OrderItems)
                {
                    // Obter o produto novamente para garantir consistência (se necessário)
                    var product = await GetProductAsync(orderItemDto.ProductId, result);

                    // Atualiza saldo do usuário e estoque do produto
                    UpdateUserAndProduct(user.ToroAccount, product, orderItemDto);

                    // Persistência de dados
                    await _productRepository.UpdateProductAsync(product);

                    // Atualizar ou criar o ativo do usuário
                    await UpdateUserAssetsAsync(user.Id, orderItemDto.ProductId, orderItemDto.Quantity, product.Name);
                }

                // Persistência do usuário atualizado (com saldo atualizado)
                await _userRepository.UpdateUserAsync(user);

                // Criar e salvar a nova ordem de compra
                await CreateOrderAsync(user, orderDto);

                // Compra realizada com sucesso
                result.Success = true;
                result.Message = "Compra realizada com sucesso!";
                return result;
            }
            catch (Exception ex)
            {
                return HandleException(ex, result);
            }
        }

        // Método auxiliar para atualizar ou criar um ativo para o usuário
        private async Task UpdateUserAssetsAsync(int userId, int productId, int quantity, string productName)
        {
            // Verifica se o usuário já possui esse ativo
            var userAsset = await _userAssetRepository.GetByUserIdAndProductIdAsync(userId, productId);

            if (userAsset != null)
            {
                // Atualiza a quantidade do ativo existente
                userAsset.Quantity += quantity;
                await _userAssetRepository.UpdateAsync(userAsset);
            }
            else
            {
                // Cria um novo ativo para o usuário
                var newUserAsset = new UserAsset
                {
                    UserId = userId,
                    ProductId = productId,
                    Quantity = quantity,
                    ProductName = productName
                };
                await _userAssetRepository.AddAsync(newUserAsset);
            }
        }


       
        public async Task<List<OrderDto>> GetOrdersByUserIdAsync(int userId)
        {
            var orders = await _orderRepository.GetOrdersByUserIdWithProduct(userId);

            return orders.Select(order => new OrderDto
            {
                Id = order.Id,
                UserId = order.UserId,
                OrderDate = order.OrderDate,
                OrderItems = order.OrderItems.Select(item => new OrderItemDto
                {
                    ProductId = item.ProductId,
                    ProductName = item.Product != null ? item.Product.Name : "Produto não encontrado",
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice
                }).ToList()
            }).ToList();
        }

        // Método para buscar o usuário e validar sua existência
        private async Task<User> GetUserAsync(int userId, OrderProcessingResult result)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                result.Success = false;
                result.Message = "Usuário não encontrado.";
                result.Errors.Add("Usuário não encontrado.");
            }
            result.Success = true;
            return user;
        }

        // Método para buscar o produto e validar sua existência
        private async Task<Product> GetProductAsync(int productId, OrderProcessingResult result)
        {
            var product = await _productRepository.GetProductByIdAsync(productId);
            if (product == null)
            {
                result.Success = false;
                result.Message = $"Produto com ID {productId} não encontrado.";
                result.Errors.Add($"Produto com ID {productId} não encontrado.");
            }
            return product;
        }

        // Valida se o produto tem estoque suficiente e se o usuário tem saldo suficiente
        private bool ValidateProductAndUser(ToroAccount account, Product product, OrderItemDto orderItemDto, OrderProcessingResult result)
        {
            if (!product.HasSufficientStock(orderItemDto.Quantity))
            {
                result.Success = false;
                result.Message = $"Estoque insuficiente para o produto {product.Name}.";
                result.Errors.Add($"Estoque insuficiente para o produto {product.Name}.");
                return false;
            }

            if (!account.HasSufficientBalance(product.UnitPrice * orderItemDto.Quantity))
            {
                result.Success = false;
                result.Message = $"Saldo insuficiente para comprar o produto {product.Name}.";
                result.Errors.Add($"Saldo insuficiente para comprar o produto {product.Name}.");
                return false;
            }

            return true;
        }

        // Atualiza o saldo do usuário e o estoque do produto
        private void UpdateUserAndProduct(ToroAccount account, Product product, OrderItemDto orderItemDto)
        {
            account.DebitBalance(product.UnitPrice * orderItemDto.Quantity);
            product.DebitStock(orderItemDto.Quantity);
        }

        // Cria a nova ordem de compra no repositório
        private async Task CreateOrderAsync(User user, OrderDto orderDto)
        {
            var newOrder = new Order
            {
                UserId = user.Id,
                OrderDate = DateTime.UtcNow,
                OrderItems = orderDto.OrderItems.Select(item => new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice
                }).ToList()
            };

            await _orderRepository.CreateOrderAsync(newOrder);
        }

        // Lida com exceções
        private OrderProcessingResult HandleException(Exception ex, OrderProcessingResult result)
        {
            result.Success = false;
            result.Message = $"Erro ao processar a compra: {ex.Message}";
            result.Errors.Add(ex.Message);
            return result;
        }
    }

}
