using Xunit;
using Moq;
using FluentAssertions;
using FixedAssets.Application.Services;
using FixedAssets.Domain.Entities;
using FixedAssets.Infrastructure.Interfaces;

namespace FixedAssets.Application.Tests.Services
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IProductRepository> _productRepositoryMock; 
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _productRepositoryMock = new Mock<IProductRepository>(); 

            _userService = new UserService(_userRepositoryMock.Object, _productRepositoryMock.Object); 
        }

        [Fact]
        public async Task GetUserById_ShouldReturnUser_WhenUserExists()
        {
            try
            {
                // Arrange
                var userId = 1;
                var expectedUser = new User
                {
                    Id = userId,
                    Name = "Test User",
                    Balance = 1000,
                    CPF = "123.456.789-00", 
                    Orders = new List<Order>(), 
                    Assets = new List<UserAsset>()  
                };

                _userRepositoryMock.Setup(repo => repo.GetUserByIdAsync(userId))
                    .ReturnsAsync(expectedUser);

                // Act
                var user = await _userService.GetUserByIdAsync(userId);

                // Assert
                user.Should().BeEquivalentTo(expectedUser, options => options.Excluding(u => u.Assets));
            }
            catch (Exception ex)
            {
               
                Assert.True(false, $"Test failed with exception: {ex.Message} {ex.StackTrace}");
            }
        }



        [Fact]
        public async Task GetUserById_ShouldReturnNull_WhenUserDoesNotExist()
        {
            // Arrange
            var userId = 1;
            _userRepositoryMock.Setup(repo => repo.GetUserByIdAsync(userId))
                .ReturnsAsync((User)null);

            // Act
            var user = await _userService.GetUserByIdAsync(userId);

            // Assert
            user.Should().BeNull();
        }
    }
}
