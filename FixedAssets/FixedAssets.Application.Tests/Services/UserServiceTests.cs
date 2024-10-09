using Xunit;
using Moq;
using FluentAssertions;
using FixedAssets.Application.Services;
using FixedAssets.Application.DTOs;
using FixedAssets.Domain.Entities;
using FixedAssets.Infrastructure.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using FixedAssets.Application.Interfaces;

namespace FixedAssets.Application.Tests.Services
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly Mock<IToroAccountService> _toroAccountServiceMock; // Adiciona mock para o ToroAccountService
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _productRepositoryMock = new Mock<IProductRepository>();
            _toroAccountServiceMock = new Mock<IToroAccountService>(); // Inicializa mock

            _userService = new UserService(_userRepositoryMock.Object, _productRepositoryMock.Object, _toroAccountServiceMock.Object);
        }

        [Fact]
        public async Task GetUserById_ShouldReturnUserDto_WhenUserExists()
        {
            // Arrange
            var userId = 1;
            var expectedUser = new User
            {
                Id = userId,
                Name = "Test User",
                Balance = 0,
                CPF = "123.456.789-00",
                Orders = new List<Order>(),
                Assets = new List<UserAsset>()
            };

            _userRepositoryMock.Setup(repo => repo.GetUserByIdAsync(userId))
                .ReturnsAsync(expectedUser);

            var expectedUserDto = new UserDto
            {
                Id = expectedUser.Id,
                Name = expectedUser.Name,
                Balance = expectedUser.Balance,
                CPF = expectedUser.CPF,
                Orders = new List<OrderDto>(),
                Assets = new List<UserAssetDto>()
            };

            // Act
            var userDto = await _userService.GetUserByIdAsync(userId);

            // Assert
            userDto.Should().BeEquivalentTo(expectedUserDto, options => options.ExcludingMissingMembers());
        }

        [Fact]
        public async Task GetUserById_ShouldReturnNull_WhenUserDoesNotExist()
        {
            // Arrange
            var userId = 1;
            _userRepositoryMock.Setup(repo => repo.GetUserByIdAsync(userId))
                .ReturnsAsync((User)null);

            // Act
            var userDto = await _userService.GetUserByIdAsync(userId);

            // Assert
            userDto.Should().BeNull();
        }
    }
}
