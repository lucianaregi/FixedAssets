using Microsoft.EntityFrameworkCore;
using Xunit;
using FluentAssertions;
using FixedAssets.Application.Services;
using FixedAssets.Infrastructure.Repositories;
using FixedAssets.Infrastructure.Persistence;
using FixedAssets.Domain.Entities;
using FixedAssets.Application.DTOs;
using FixedAssets.Application.Interfaces;
using Microsoft.EntityFrameworkCore.InMemory;

namespace FixedAssets.Application.Tests.Integration
{
    public class ProductPurchaseIntegrationTests
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserService _userService;
        private readonly IProductService _productService;
        private readonly IOrderService _orderService;

        public ProductPurchaseIntegrationTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestPurchaseDatabase") // Banco de dados em memória
                .Options;

            _context = new ApplicationDbContext(options);

            // Repositórios
            var userRepository = new UserRepository(_context);
            var productRepository = new ProductRepository(_context);
            var orderRepository = new OrderRepository(_context);

            // Serviços
            _userService = new UserService(userRepository, productRepository);
            _productService = new ProductService(productRepository);
            _orderService = new OrderService(userRepository, productRepository, orderRepository);
        }

        [Fact(DisplayName = "Deve processar a compra de um produto corretamente")]
        public async Task ProcessarCompraProduto()
        {
            // Arrange - Preparando dados
            var user = new User { Id = 1, Name = "Teste Usuário", CPF = "12345678901", Balance = 1000 };
            var product = new Product { Id = 1, Name = "Produto Teste", Indexer = "CDI", Tax= 6.5m, Stock = 10, UnitPrice = 50 };
         
            _context.Users.Add(user);
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            var orderDto = new OrderDto
            {
                UserId = 1,
                OrderItems = new List<OrderItemDto>
                {
                    new OrderItemDto { ProductId = 1, Quantity = 2, UnitPrice = 50 }
                }
            };

            // Act - Executando o processo de compra
            var result = await _orderService.ProcessOrderAsync(orderDto);

            // Assert - Verificando o resultado
            result.Should().BeTrue("A compra deve ser processada com sucesso.");

            var updatedUser = await _context.Users.FindAsync(1);
            var updatedProduct = await _context.Products.FindAsync(1);

            updatedUser.Balance.Should().Be(900, "O saldo do usuário deve ser atualizado após a compra.");
            updatedProduct.Stock.Should().Be(8, "O estoque do produto deve ser atualizado após a compra.");
        }
    }
}
