using Microsoft.EntityFrameworkCore;
using Xunit;
using FluentAssertions;
using FixedAssets.Application.Services;
using FixedAssets.Infrastructure.Repositories;
using FixedAssets.Infrastructure.Persistence;
using FixedAssets.Domain.Entities;
using FixedAssets.Application.DTOs;
using FixedAssets.Application.Interfaces;
using Moq;
using FixedAssets.Infrastructure.Interfaces;

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

            // Mock do IUserAssetRepository
            var userAssetRepositoryMock = new Mock<IUserAssetRepository>();

            // Mock do IToroAccountService
            var toroAccountServiceMock = new Mock<IToroAccountService>();

            // Configurar o mock para retorno de uma conta válida
            toroAccountServiceMock.Setup(service => service.GetAccountByUserIdAsync(It.IsAny<int>()))
                                  .ReturnsAsync(new ToroAccountDto { AccountNumber = "00001", Balance = 1000 });

            // Serviços
            _userService = new UserService(userRepository, productRepository, toroAccountServiceMock.Object);
            _productService = new ProductService(productRepository);
            _orderService = new OrderService(userRepository, productRepository, orderRepository, userAssetRepositoryMock.Object); 
        }

        [Fact(DisplayName = "Deve processar a compra de um produto corretamente")]
        public async Task ProcessarCompraProduto()
        {
            // Arrange - Preparando dados
            var user = new User
            {
                Id = 1,
                Name = "Teste Usuário",
                CPF = "12345678901",
                Email = "teste@usuario.com",
                PasswordHash = "hashedpassword",
                ToroAccount = new ToroAccount
                {
                    AccountNumber = "00001",
                    Balance = 1000m
                }
            };
            var product = new Product { Id = 1, Name = "Produto Teste", Indexer = "CDI", Tax = 6.5m, Stock = 10, UnitPrice = 50m };

            _context.Users.Add(user);
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            var orderDto = new OrderDto
            {
                UserId = 1,
                OrderItems = new List<OrderItemDto>
        {
            new OrderItemDto { ProductId = 1, Quantity = 2, UnitPrice = 50m }
        }
            };

            // Act - Executando o processo de compra
            var result = await _orderService.ProcessOrderAsync(orderDto);

            // Assert - Verificando o resultado
            result.Success.Should().BeTrue("A compra deve ser processada com sucesso.");

            // Recarregando o usuário e o produto do banco de dados
            var updatedUser = await _context.Users
                .Include(u => u.ToroAccount)
                .FirstOrDefaultAsync(u => u.Id == 1);
            var updatedProduct = await _context.Products.FindAsync(1);

            updatedUser.ToroAccount.Balance.Should().Be(900m, "O saldo da conta Toro do usuário deve ser atualizado após a compra.");
            updatedProduct.Stock.Should().Be(8, "O estoque do produto deve ser atualizado após a compra.");

            // Verificando se a ordem foi criada corretamente
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.UserId == user.Id);
            order.Should().NotBeNull("Uma nova ordem deve ser criada");
            order.OrderItems.Should().HaveCount(1, "A ordem deve ter um item");
            order.OrderItems.First().Quantity.Should().Be(2, "A quantidade do item deve ser 2");
            order.OrderItems.First().UnitPrice.Should().Be(50m, "O preço unitário deve ser 50");
        }
    }
}
