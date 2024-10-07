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
    public class ProductControllerTests
    {
        private readonly Mock<IProductService> _productServiceMock;
        private readonly ProductController _controller;

        public ProductControllerTests()
        {
            _productServiceMock = new Mock<IProductService>();
            _controller = new ProductController(_productServiceMock.Object);
        }

        [Fact]
        public async Task GetProducts_ShouldReturnOkWithProducts_WhenProductsExist()
        {
            // Arrange
            var products = new List<ProductDto>
            {
                new ProductDto { Id = 1, Name = "Product 1", UnitPrice = 100, Stock = 10 },
                new ProductDto { Id = 2, Name = "Product 2", UnitPrice = 200, Stock = 20 }
            };
            _productServiceMock.Setup(service => service.GetProductsAsync())
                .ReturnsAsync(products);

            // Act
            var result = await _controller.GetProducts();

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);
            okResult.Value.Should().BeEquivalentTo(products);
        }

        [Fact]
        public async Task GetProduct_ShouldReturnOkWithProduct_WhenProductExists()
        {
            // Arrange
            var productId = 1;
            var product = new ProductDto { Id = productId, Name = "Product 1", UnitPrice = 100, Stock = 10 };
            _productServiceMock.Setup(service => service.GetProductByIdAsync(productId))
                .ReturnsAsync(product);

            // Act
            var result = await _controller.GetProduct(productId);

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);
            okResult.Value.Should().BeEquivalentTo(product);
        }

       
        [Fact]
        public async Task GetProduct_ShouldReturnNotFound_WhenProductDoesNotExist()
        {
            // Arrange
            var productId = 1;
            _productServiceMock.Setup(service => service.GetProductByIdAsync(productId))
                .ReturnsAsync((ProductDto)null); 

            // Act
            var result = await _controller.GetProduct(productId);

            // Assert
            var notFoundResult = result as NotFoundObjectResult;
            notFoundResult.Should().NotBeNull(); 
            notFoundResult.Value.Should().Be("Produto não encontrado."); 
        }

    }
}
