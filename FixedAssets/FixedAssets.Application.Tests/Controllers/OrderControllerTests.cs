using Xunit;
using Moq;
using System.Threading.Tasks;
using FluentAssertions;
using FixedAssets.Api.Controllers;
using FixedAssets.Application.Interfaces;
using FixedAssets.Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace FixedAssets.Application.Tests.Controllers
{
    public class OrderControllerTests
    {
        private readonly Mock<IOrderService> _orderServiceMock;
        private readonly Mock<IOrderItemService> _orderItemServiceMock;
        private readonly OrderController _controller;

        public OrderControllerTests()
        {
            _orderServiceMock = new Mock<IOrderService>();
            _orderItemServiceMock = new Mock<IOrderItemService>();
            _controller = new OrderController(_orderServiceMock.Object, _orderItemServiceMock.Object);
        }

        [Fact]
        public async Task ProcessOrder_ShouldReturnOk_WhenOrderIsProcessedSuccessfully()
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

            _orderServiceMock.Setup(service => service.ProcessOrderAsync(orderDto))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.ProcessOrder(orderDto);

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);
            okResult.Value.Should().Be("Compra realizada com sucesso.");
        }

        [Fact]
        public async Task ProcessOrder_ShouldReturnBadRequest_WhenOrderIsInvalid()
        {
            // Arrange
            var orderDto = new OrderDto
            {
                UserId = 1,
                OrderItems = new List<OrderItemDto>
                {
                    new OrderItemDto { ProductId = 1, Quantity = 0, UnitPrice = 100 }
                }
            };

            // Act
            var result = await _controller.ProcessOrder(orderDto);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task GetOrderItems_ShouldReturnOkWithItems_WhenItemsExist()
        {
            // Arrange
            var orderId = 1;
            var orderItems = new List<OrderItemDto>
            {
                new OrderItemDto { ProductId = 1, ProductName = "Product 1", Quantity = 2, UnitPrice = 100 }
            };
            _orderItemServiceMock.Setup(service => service.GetOrderItemsByOrderIdAsync(orderId))
                .ReturnsAsync(orderItems);

            // Act
            var result = await _controller.GetOrderItems(orderId);

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);
            okResult.Value.Should().BeEquivalentTo(orderItems);
        }

        [Fact]
        public async Task GetOrderItems_ShouldReturnNotFound_WhenNoItemsExist()
        {
            // Arrange
            var orderId = 1;
            _orderItemServiceMock.Setup(service => service.GetOrderItemsByOrderIdAsync(orderId))
                .ReturnsAsync(new List<OrderItemDto>());

            // Act
            var result = await _controller.GetOrderItems(orderId);

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>();
        }
    }
}
