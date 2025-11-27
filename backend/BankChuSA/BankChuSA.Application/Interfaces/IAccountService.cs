using BankChuSA.Application.DTOs;

namespace BankChuSA.Application.Interfaces
{
    public interface IAccountService
    {
        Task<AccountDto> CreateAccountAsync(CreateAccountDto createAccountDto);
        Task<AccountDto?> GetAccountByNumberAsync(string accountNumber);
        Task<bool> AccountExistsAsync(string accountNumber);
    }
}

