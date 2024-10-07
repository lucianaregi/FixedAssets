using FluentValidation;
using FixedAssets.Domain.Entities;

namespace FixedAssets.Application.Validators
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(user => user.Name)
                .NotEmpty().WithMessage("O nome do usuário é obrigatório.");

            RuleFor(user => user.CPF)
                .NotEmpty().WithMessage("O CPF é obrigatório.")
                .Length(11).WithMessage("O CPF deve ter 11 dígitos.");

            RuleFor(user => user.Balance)
                .GreaterThanOrEqualTo(0).WithMessage("O saldo não pode ser negativo.");
        }
    }
}
