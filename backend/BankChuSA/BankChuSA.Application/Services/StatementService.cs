using BankChuSA.Application.DTOs;
using BankChuSA.Application.Interfaces;
using BankChuSA.Domain.Entities;
using System.Linq.Expressions;

namespace BankChuSA.Application.Services
{
    public class StatementService : IStatementService
    {
        private readonly IUnitOfWork _unitOfWork;

        public StatementService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<StatementDto> GetStatementAsync(string accountNumber, DateTime startDate, DateTime endDate)
        {
            var account = await _unitOfWork.Accounts.GetByExpressionAsync(
                a => a.AccountNumber == accountNumber && a.IsActive);

            if (account == null)
            {
                throw new InvalidOperationException("Conta não encontrada");
            }

            var endDateUtc = endDate.Date.AddDays(1).AddTicks(-1);

            Expression<Func<Transaction, bool>> transactionFilter = t =>
                t.AccountId == account.Id &&
                t.TransactionDate >= startDate.Date &&
                t.TransactionDate <= endDateUtc;

            var transactions = await _unitOfWork.Transactions.FindAsync(transactionFilter);
            var transactionsList = transactions.OrderBy(t => t.TransactionDate).ToList();

            var initialBalance = account.Balance - transactionsList.Sum(t => t.Amount);

            var statement = new StatementDto
            {
                AccountId = account.Id,
                AccountNumber = account.AccountNumber,
                StartDate = startDate.Date,
                EndDate = endDate.Date,
                InitialBalance = initialBalance,
                FinalBalance = account.Balance,
                Transactions = transactionsList.Select(t => new TransactionItemDto
                {
                    TransactionDate = t.TransactionDate,
                    Type = t.Type.ToString(),
                    Amount = t.Amount,
                    Description = t.Description,
                    RelatedAccountNumber = t.RelatedAccount?.AccountNumber
                }).ToList()
            };

            return statement;
        }
    }
}