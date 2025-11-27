using BankChuSA.Application.DTOs;
using BankChuSA.Application.Interfaces;
using BankChuSA.Application.Resources;
using BankChuSA.Domain.Entities;
using BankChuSA.Domain.Enums;

namespace BankChuSA.Application.Services
{
    public class TransferService : ITransferService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHolidayService _holidayService;

        public TransferService(
            IUnitOfWork unitOfWork,
            IHolidayService holidayService)
        {
            _unitOfWork = unitOfWork;
            _holidayService = holidayService;
        }

        public async Task<bool> ExecuteTransferAsync(TransferDto transferDto)
        {
            var today = DateTime.UtcNow.Date;
            var isWorkingDay = await _holidayService.IsWorkingDayAsync(today);

            if (!isWorkingDay)
            {
                throw new InvalidOperationException(ErrorMessages.TransferOnlyOnWorkingDays);
            }

            // O CommitTransactionAsync agora gerencia a transação internamente com retry
            await _unitOfWork.CommitTransactionAsync(async () =>
            {
                var fromAccount = await _unitOfWork.Accounts.GetByExpressionAsync(
                    a => a.AccountNumber == transferDto.FromAccountNumber && a.IsActive);

                if (fromAccount == null)
                {
                    throw new InvalidOperationException(ErrorMessages.SourceAccountNotFound);
                }

                var toAccount = await _unitOfWork.Accounts.GetByExpressionAsync(
                    a => a.AccountNumber == transferDto.ToAccountNumber && a.IsActive);

                if (toAccount == null)
                {
                    throw new InvalidOperationException(ErrorMessages.DestinationAccountNotFound);
                }

                if (fromAccount.Balance < transferDto.Amount)
                {
                    throw new InvalidOperationException(ErrorMessages.InsufficientBalance);
                }

                fromAccount.Balance -= transferDto.Amount;
                toAccount.Balance += transferDto.Amount;
                fromAccount.UpdatedAt = DateTime.UtcNow;
                toAccount.UpdatedAt = DateTime.UtcNow;

                await _unitOfWork.Accounts.UpdateAsync(fromAccount);
                await _unitOfWork.Accounts.UpdateAsync(toAccount);

                var transactionDate = DateTime.UtcNow;

                var debitTransaction = new Transaction
                {
                    Id = Guid.NewGuid(),
                    AccountId = fromAccount.Id,
                    Type = TransactionType.Transfer,
                    Amount = -transferDto.Amount,
                    Description = transferDto.Description,
                    TransactionDate = transactionDate,
                    RelatedAccountId = toAccount.Id
                };

                var creditTransaction = new Transaction
                {
                    Id = Guid.NewGuid(),
                    AccountId = toAccount.Id,
                    Type = TransactionType.Transfer,
                    Amount = transferDto.Amount,
                    Description = transferDto.Description,
                    TransactionDate = transactionDate,
                    RelatedAccountId = fromAccount.Id
                };

                await _unitOfWork.Transactions.AddAsync(debitTransaction);
                await _unitOfWork.Transactions.AddAsync(creditTransaction);
            });

            return true;
        }
    }

}