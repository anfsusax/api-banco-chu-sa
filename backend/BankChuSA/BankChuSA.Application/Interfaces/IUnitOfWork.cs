using BankChuSA.Domain.Entities;

namespace BankChuSA.Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Account> Accounts { get; }
        IRepository<Transaction> Transactions { get; }
        IRepository<User> Users { get; }
        
        Task<int> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task CommitTransactionAsync(Func<Task> operation);
        Task RollbackTransactionAsync();
    }
}

