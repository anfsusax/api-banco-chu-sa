using BankChuSA.Application.DTOs;
using BankChuSA.Application.Interfaces;
using BankChuSA.Domain.Entities;

namespace BankChuSA.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AccountService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<AccountDto> CreateAccountAsync(CreateAccountDto createAccountDto)
        {
            var accountNumber = GenerateAccountNumber();

            while (await AccountExistsAsync(accountNumber))
            {
                accountNumber = GenerateAccountNumber();
            }

            var account = new Account
            {
                Id = Guid.NewGuid(),
                AccountNumber = accountNumber,
                OwnerName = createAccountDto.OwnerName,
                DocumentNumber = createAccountDto.DocumentNumber,
                Balance = createAccountDto.InitialBalance,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            await _unitOfWork.Accounts.AddAsync(account);
            await _unitOfWork.SaveChangesAsync();

            return new AccountDto
            {
                Id = account.Id,
                AccountNumber = account.AccountNumber,
                OwnerName = account.OwnerName,
                DocumentNumber = account.DocumentNumber,
                Balance = account.Balance,
                CreatedAt = account.CreatedAt
            };
        }

        public async Task<AccountDto?> GetAccountByNumberAsync(string accountNumber)
        {
            var account = await _unitOfWork.Accounts.GetByExpressionAsync(a => a.AccountNumber == accountNumber && a.IsActive);

            if (account == null)
                return null;

            return new AccountDto
            {
                Id = account.Id,
                AccountNumber = account.AccountNumber,
                OwnerName = account.OwnerName,
                DocumentNumber = account.DocumentNumber,
                Balance = account.Balance,
                CreatedAt = account.CreatedAt
            };
        }

        public async Task<bool> AccountExistsAsync(string accountNumber)
        {
            return await _unitOfWork.Accounts.ExistsAsync(a => a.AccountNumber == accountNumber);
        }

        private string GenerateAccountNumber()
        {
            var random = new Random();
            return random.Next(100000, 999999).ToString();
        }
    }
}

