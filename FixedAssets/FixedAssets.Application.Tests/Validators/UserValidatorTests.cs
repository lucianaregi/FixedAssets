using Xunit;
using FluentValidation.TestHelper;
using FixedAssets.Domain.Entities;
using FixedAssets.Application.Validators;

namespace FixedAssets.Application.Tests.Validators
{
    public class UserValidatorTests
    {
        private readonly UserValidator _validator;

        public UserValidatorTests()
        {
            _validator = new UserValidator();
        }

        [Fact]
        public void Should_HaveError_When_NameIsEmpty()
        {
            var user = new User { Name = "" };
            var result = _validator.TestValidate(user);
            result.ShouldHaveValidationErrorFor(u => u.Name)
                  .WithErrorMessage("O nome do usuário é obrigatório.");
        }

        [Fact]
        public void Should_HaveError_When_CPFIsInvalid()
        {
            var user = new User { CPF = "123" };  // CPF inválido (menos de 11 dígitos)
            var result = _validator.TestValidate(user);
            result.ShouldHaveValidationErrorFor(u => u.CPF)
                  .WithErrorMessage("O CPF deve ter 11 dígitos.");
        }

        [Fact]
        public void Should_NotHaveError_When_UserIsValid()
        {
            var user = new User
            {
                Name = "Teste",
                CPF = "12345678901", // CPF válido
                Balance = 1000
            };

            var result = _validator.TestValidate(user);
            result.ShouldNotHaveValidationErrorFor(u => u.Name);
            result.ShouldNotHaveValidationErrorFor(u => u.CPF);
        }

        [Fact]
        public void Should_HaveError_When_BalanceIsNegative()
        {
            var user = new User { Balance = -10 }; // Saldo negativo
            var result = _validator.TestValidate(user);
            result.ShouldHaveValidationErrorFor(u => u.Balance)
                  .WithErrorMessage("O saldo não pode ser negativo.");
        }
    }
}
