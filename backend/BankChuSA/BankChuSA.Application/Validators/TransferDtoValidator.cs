using BankChuSA.Application.DTOs;
using FluentValidation;

namespace BankChuSA.Application.Validators
{
    public class TransferDtoValidator : AbstractValidator<TransferDto>
    {
        public TransferDtoValidator()
        {
            RuleFor(x => x.FromAccountNumber)
                .NotEmpty().WithMessage("Conta de origem é obrigatória");

            RuleFor(x => x.ToAccountNumber)
                .NotEmpty().WithMessage("Conta de destino é obrigatória")
                .NotEqual(x => x.FromAccountNumber).WithMessage("Conta de origem e destino não podem ser iguais");

            RuleFor(x => x.Amount)
                .GreaterThan(0).WithMessage("Valor da transferência deve ser maior que zero");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Descrição não pode exceder 500 caracteres");
        }
    }
}

