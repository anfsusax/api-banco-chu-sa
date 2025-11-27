using BankChuSA.Application.DTOs;

namespace BankChuSA.Application.Interfaces
{
    public interface ITransferService
    {
        Task<bool> ExecuteTransferAsync(TransferDto transferDto);
    }
}

