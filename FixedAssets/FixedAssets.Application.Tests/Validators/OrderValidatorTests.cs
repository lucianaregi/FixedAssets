using Xunit;
using FluentValidation.TestHelper;
using FixedAssets.Domain.Entities;
using FixedAssets.Application.Validators;

namespace FixedAssets.Application.Tests.Validators
{
    public class OrderValidatorTests
    {
        private readonly OrderValidator _validator;

        public OrderValidatorTests()
        {
            _validator = new OrderValidator();
        }

        [Fact]
        public void Should_HaveError_When_UserIdIsZero()
        {
            var order = new Order { UserId = 0 };
            var result = _validator.TestValidate(order);
            result.ShouldHaveValidationErrorFor(o => o.UserId)
                  .WithErrorMessage("O ID do usuário é obrigatório.");
        }

        [Fact]
        public void Should_HaveError_When_OrderItemsAreEmpty()
        {
            var order = new Order { OrderItems = new List<OrderItem>() };
            var result = _validator.TestValidate(order);
            result.ShouldHaveValidationErrorFor(o => o.OrderItems)
                  .WithErrorMessage("A ordem deve conter pelo menos um item.");
        }

        [Fact]
        public void Should_NotHaveError_When_OrderIsValid()
        {
            var order = new Order
            {
                UserId = 1,
                OrderItems = new List<OrderItem>
                {
                    new OrderItem { ProductId = 1, Quantity = 2, UnitPrice = 100 }
                }
            };

            var result = _validator.TestValidate(order);
            result.ShouldNotHaveValidationErrorFor(o => o.UserId);
            result.ShouldNotHaveValidationErrorFor(o => o.OrderItems);
        }
    }
}
