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
        private readonly OrderService _orderService;

        public OrderServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _productRepositoryMock = new Mock<IProductRepository>();
            _orderRepositoryMock = new Mock<IOrderRepository>();

            _orderService = new OrderService(_userRepositoryMock.Object, _productRepositoryMock.Object, _orderRepositoryMock.Object);
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

            var user = new User { Id = 1, Balance = 500 };
            var product = new Product { Id = 1, Stock = 10, UnitPrice = 100 };

            _userRepositoryMock.Setup(repo => repo.GetUserByIdAsync(orderDto.UserId)).ReturnsAsync(user);
            _productRepositoryMock.Setup(repo => repo.GetProductByIdAsync(1)).ReturnsAsync(product);
            _orderRepositoryMock.Setup(repo => repo.CreateOrderAsync(It.IsAny<Order>())).Returns(Task.CompletedTask);

            // Act
            var result = await _orderService.ProcessOrderAsync(orderDto);

            // Assert
            result.Should().BeTrue();
            user.Balance.Should().Be(300); // Verifica se o saldo foi debitado corretamente
            product.Stock.Should().Be(8);  // Verifica se o estoque foi debitado corretamente

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
            result.Should().BeFalse();
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
            result.Should().BeFalse();
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
            result.Should().BeFalse();
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
            result.Should().BeFalse();
            _orderRepositoryMock.Verify(repo => repo.CreateOrderAsync(It.IsAny<Order>()), Times.Never);
        }

        [Fact]
        public async Task GetOrdersByUserId_ShouldReturnListOfOrders_WhenOrdersExist()
        {
            // Arrange
            var userId = 1;
            var expectedOrders = new List<Order>
            {
                new Order { Id = 1, UserId = userId, OrderItems = new List<OrderItem> { new OrderItem { ProductId = 1, Quantity = 2, UnitPrice = 100 } } },
                new Order { Id = 2, UserId = userId, OrderItems = new List<OrderItem> { new OrderItem { ProductId = 2, Quantity = 3, UnitPrice = 150 } } }
            };

            _orderRepositoryMock.Setup(repo => repo.GetOrdersByUserIdWithProduct(userId)).ReturnsAsync(expectedOrders);

            // Act
            var orders = await _orderService.GetOrdersByUserIdAsync(userId);

            // Assert
            orders.Should().BeEquivalentTo(expectedOrders, options => options.ExcludingMissingMembers());
        }
    }
}
