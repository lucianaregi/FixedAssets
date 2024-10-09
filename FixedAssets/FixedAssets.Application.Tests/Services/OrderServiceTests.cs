using Xunit;
using Moq;
using FluentAssertions;
using FixedAssets.Application.Services;
using FixedAssets.Application.DTOs;
using FixedAssets.Infrastructure.Interfaces;
using FixedAssets.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FixedAssets.Application.Tests.Services
{
    public class OrderServiceTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly Mock<IOrderRepository> _orderRepositoryMock;
        private readonly Mock<IUserAssetRepository> _userAssetRepositoryMock;
        private readonly OrderService _orderService;

        public OrderServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _productRepositoryMock = new Mock<IProductRepository>();
            _orderRepositoryMock = new Mock<IOrderRepository>();
            _userAssetRepositoryMock = new Mock<IUserAssetRepository>();

            _orderService = new OrderService(
                _userRepositoryMock.Object,
                _productRepositoryMock.Object,
                _orderRepositoryMock.Object,
                _userAssetRepositoryMock.Object 
            );
        }

        [Fact]
        public async Task ProcessOrder_ShouldReturnTrue_WhenOrderIsProcessedSuccessfully()
        {
            // Arrange
            var orderDto = new OrderDto
            {
                UserId = 1,
                OrderItems = new List<OrderItemDto>
        {
            new OrderItemDto { ProductId = 1, Quantity = 2, UnitPrice = 100 }
        }
            };

            var toroAccount = new ToroAccount { Balance = 500 }; // Criar a ToroAccount com saldo
            var user = new User { Id = 1, ToroAccount = toroAccount }; // Associar o ToroAccount ao User
            var product = new Product { Id = 1, Stock = 10, UnitPrice = 100 };

            
            _userRepositoryMock.Setup(repo => repo.GetUserByIdAsync(orderDto.UserId)).ReturnsAsync(user);
            _productRepositoryMock.Setup(repo => repo.GetProductByIdAsync(1)).ReturnsAsync(product);
            _orderRepositoryMock.Setup(repo => repo.CreateOrderAsync(It.IsAny<Order>())).Returns(Task.CompletedTask);

            // Act
            var result = await _orderService.ProcessOrderAsync(orderDto);

            // Assert
            result.Success.Should().BeTrue(); // Espera-se que o resultado seja verdadeiro
            user.ToroAccount.Balance.Should().Be(300); // Verifica se o saldo da ToroAccount foi atualizado corretamente
            product.Stock.Should().Be(8);  // Verifica se o estoque do produto foi debitado corretamente

            // Verificar se os métodos corretos foram chamados
            _userRepositoryMock.Verify(repo => repo.UpdateUserAsync(user), Times.Once);
            _productRepositoryMock.Verify(repo => repo.UpdateProductAsync(product), Times.Once);
            _orderRepositoryMock.Verify(repo => repo.CreateOrderAsync(It.IsAny<Order>()), Times.Once);
        }


        [Fact]
        public async Task ProcessOrder_ShouldReturnFalse_WhenUserDoesNotExist()
        {
            // Arrange
            var orderDto = new OrderDto
            {
                UserId = 1,
                OrderItems = new List<OrderItemDto> { new OrderItemDto { ProductId = 1, Quantity = 1, UnitPrice = 100 } }
            };

            _userRepositoryMock.Setup(repo => repo.GetUserByIdAsync(orderDto.UserId)).ReturnsAsync((User)null);

            // Act
            var result = await _orderService.ProcessOrderAsync(orderDto);

            // Assert
            result.Success.Should().BeFalse(); // Trocar para BeFalse()
            _orderRepositoryMock.Verify(repo => repo.CreateOrderAsync(It.IsAny<Order>()), Times.Never);
        }

        [Fact]
        public async Task ProcessOrder_ShouldReturnFalse_WhenProductDoesNotExist()
        {
            // Arrange
            var orderDto = new OrderDto
            {
                UserId = 1,
                OrderItems = new List<OrderItemDto> { new OrderItemDto { ProductId = 1, Quantity = 1, UnitPrice = 100 } }
            };

            var user = new User { Id = 1, Balance = 500 };

            _userRepositoryMock.Setup(repo => repo.GetUserByIdAsync(orderDto.UserId)).ReturnsAsync(user);
            _productRepositoryMock.Setup(repo => repo.GetProductByIdAsync(1)).ReturnsAsync((Product)null);

            // Act
            var result = await _orderService.ProcessOrderAsync(orderDto);

            // Assert
            result.Success.Should().BeFalse(); // Trocar para BeFalse()
            _orderRepositoryMock.Verify(repo => repo.CreateOrderAsync(It.IsAny<Order>()), Times.Never);
        }

        [Fact]
        public async Task ProcessOrder_ShouldReturnFalse_WhenUserHasInsufficientBalance()
        {
            // Arrange
            var orderDto = new OrderDto
            {
                UserId = 1,
                OrderItems = new List<OrderItemDto> { new OrderItemDto { ProductId = 1, Quantity = 5, UnitPrice = 200 } }
            };

            var user = new User { Id = 1, Balance = 300 };
            var product = new Product { Id = 1, Stock = 10, UnitPrice = 200 };

            _userRepositoryMock.Setup(repo => repo.GetUserByIdAsync(orderDto.UserId)).ReturnsAsync(user);
            _productRepositoryMock.Setup(repo => repo.GetProductByIdAsync(1)).ReturnsAsync(product);

            // Act
            var result = await _orderService.ProcessOrderAsync(orderDto);

            // Assert
            result.Success.Should().BeFalse(); // Trocar para BeFalse()
            _orderRepositoryMock.Verify(repo => repo.CreateOrderAsync(It.IsAny<Order>()), Times.Never);
        }

        [Fact]
        public async Task ProcessOrder_ShouldReturnFalse_WhenProductHasInsufficientStock()
        {
            // Arrange
            var orderDto = new OrderDto
            {
                UserId = 1,
                OrderItems = new List<OrderItemDto> { new OrderItemDto { ProductId = 1, Quantity = 15, UnitPrice = 200 } }
            };

            var user = new User { Id = 1, Balance = 4000 };
            var product = new Product { Id = 1, Stock = 10, UnitPrice = 200 };

            _userRepositoryMock.Setup(repo => repo.GetUserByIdAsync(orderDto.UserId)).ReturnsAsync(user);
            _productRepositoryMock.Setup(repo => repo.GetProductByIdAsync(1)).ReturnsAsync(product);

            // Act
            var result = await _orderService.ProcessOrderAsync(orderDto);

            // Assert
            result.Success.Should().BeFalse(); // Trocar para BeFalse()
            _orderRepositoryMock.Verify(repo => repo.CreateOrderAsync(It.IsAny<Order>()), Times.Never);
        }

    }
}
