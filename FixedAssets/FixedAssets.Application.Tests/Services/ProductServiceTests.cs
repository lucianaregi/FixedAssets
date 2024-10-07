using Xunit;
using Moq;
using FluentAssertions;
using FixedAssets.Application.Services;
using FixedAssets.Application.Interfaces;
using FixedAssets.Application.DTOs;
using FixedAssets.Domain.Entities;
using FixedAssets.Infrastructure.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FixedAssets.Application.Tests.Services
{
    public class ProductServiceTests
    {
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly ProductService _productService;

        public ProductServiceTests()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _productService = new ProductService(_productRepositoryMock.Object);
        }

        [Fact]
        public async Task GetProductsAsync_ShouldReturnListOfProducts_WhenProductsExist()
        {
            // Arrange
            var expectedProducts = new List<Product>
            {
                new Product { Id = 1, Name = "Produto 1", UnitPrice = 100, Stock = 10 },
                new Product { Id = 2, Name = "Produto 2", UnitPrice = 200, Stock = 20 }
            };

            _productRepositoryMock.Setup(repo => repo.GetAllProductsAsync())
                .ReturnsAsync(expectedProducts);

            var expectedProductDtos = expectedProducts.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                UnitPrice = p.UnitPrice,
                Stock = p.Stock
            }).ToList();

            // Act
            var productDtos = await _productService.GetProductsAsync();

            // Assert
            productDtos.Should().BeEquivalentTo(expectedProductDtos, options => options.ExcludingMissingMembers());
        }

        [Fact]
        public async Task GetProductByIdAsync_ShouldReturnProduct_WhenProductExists()
        {
            // Arrange
            var productId = 1;
            var expectedProduct = new Product { Id = productId, Name = "Produto 1", UnitPrice = 100, Stock = 10 };

            _productRepositoryMock.Setup(repo => repo.GetProductByIdAsync(productId))
                .ReturnsAsync(expectedProduct);

            var expectedProductDto = new ProductDto
            {
                Id = expectedProduct.Id,
                Name = expectedProduct.Name,
                UnitPrice = expectedProduct.UnitPrice,
                Stock = expectedProduct.Stock
            };

            // Act
            var productDto = await _productService.GetProductByIdAsync(productId);

            // Assert
            productDto.Should().BeEquivalentTo(expectedProductDto, options => options.ExcludingMissingMembers());
        }

        [Fact]
        public async Task GetProductByIdAsync_ShouldReturnNull_WhenProductDoesNotExist()
        {
            // Arrange
            var productId = 1;

            _productRepositoryMock.Setup(repo => repo.GetProductByIdAsync(productId))
                .ReturnsAsync((Product)null);

            // Act
            var productDto = await _productService.GetProductByIdAsync(productId);

            // Assert
            productDto.Should().BeNull();
        }
    }
}
