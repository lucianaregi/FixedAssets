using FluentValidation;
using FixedAssets.Domain.Entities;

namespace FixedAssets.Application.Validators
{
    public class OrderValidator : AbstractValidator<Order>
    {
        public OrderValidator()
        {
            RuleFor(order => order.UserId)
                .GreaterThan(0).WithMessage("O ID do usuário é obrigatório.");

            RuleFor(order => order.OrderItems)
                .NotEmpty().WithMessage("A ordem deve conter pelo menos um item.");

            RuleForEach(order => order.OrderItems).ChildRules(orderItem =>
            {
                orderItem.RuleFor(oi => oi.Quantity)
                    .GreaterThan(0).WithMessage("A quantidade deve ser maior que zero.");
                orderItem.RuleFor(oi => oi.UnitPrice)
                    .GreaterThan(0).WithMessage("O preço unitário deve ser maior que zero.");
            });
        }
    }
}
