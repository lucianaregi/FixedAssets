using Xunit;
using Moq;
using System.Threading.Tasks;
using FluentAssertions;
using FixedAssets.Api.Controllers;
using FixedAssets.Application.Interfaces;
using FixedAssets.Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace FixedAssets.Application.Tests.Controllers
{
    public class UserControllerTests
    {
        private readonly Mock<IUserService> _userServiceMock;
        private readonly Mock<IProductService> _productServiceMock;
        private readonly Mock<IOrderService> _orderServiceMock;
        private readonly Mock<IToroAccountService> _toroAccountServiceMock;
        private readonly UserController _controller;

        public UserControllerTests()
        {
            _userServiceMock = new Mock<IUserService>();
            _productServiceMock = new Mock<IProductService>();
            _orderServiceMock = new Mock<IOrderService>();
            _toroAccountServiceMock = new Mock<IToroAccountService>();

            _controller = new UserController(
                _userServiceMock.Object,
                _productServiceMock.Object,
                _orderServiceMock.Object,
                _toroAccountServiceMock.Object
            );
        }

        [Fact]
        public async Task GetUser_ShouldReturnOk_WhenUserExists()
        {
            // Arrange
            var userId = 1;
            var userDto = new UserDto { Id = userId, Name = "Test User", Balance = 1000 };
            _userServiceMock.Setup(service => service.GetUserByIdAsync(userId))
                .ReturnsAsync(userDto);

            // Act
            var result = await _controller.GetUser(userId);

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);
            okResult.Value.Should().BeEquivalentTo(userDto);
        }

        [Fact]
        public async Task GetUser_ShouldReturnNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            var userId = 1;
            _userServiceMock.Setup(service => service.GetUserByIdAsync(userId))
                .ReturnsAsync((UserDto)null);

            // Act
            var result = await _controller.GetUser(userId);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }
    }
}
