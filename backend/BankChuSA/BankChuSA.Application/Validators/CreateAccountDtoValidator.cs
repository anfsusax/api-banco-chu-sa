using BankChuSA.Application.DTOs;
using FluentValidation;

namespace BankChuSA.Application.Validators
{
    public class CreateAccountDtoValidator : AbstractValidator<CreateAccountDto>
    {
        public CreateAccountDtoValidator()
        {
            RuleFor(x => x.OwnerName)
                .NotEmpty().WithMessage("Nome do titular é obrigatório")
                .MaximumLength(200).WithMessage("Nome do titular não pode exceder 200 caracteres");

            RuleFor(x => x.DocumentNumber)
                .NotEmpty().WithMessage("CPF/CNPJ é obrigatório")
                .Length(11, 14).WithMessage("CPF/CNPJ deve ter 11 ou 14 caracteres");

            RuleFor(x => x.InitialBalance)
                .GreaterThanOrEqualTo(0).WithMessage("Saldo inicial não pode ser negativo");
        }
    }
}

