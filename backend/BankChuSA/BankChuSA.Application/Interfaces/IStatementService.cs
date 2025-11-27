using BankChuSA.Application.DTOs;

namespace BankChuSA.Application.Interfaces
{
    public interface IStatementService
    {
        Task<StatementDto> GetStatementAsync(string accountNumber, DateTime startDate, DateTime endDate);
    }
}

