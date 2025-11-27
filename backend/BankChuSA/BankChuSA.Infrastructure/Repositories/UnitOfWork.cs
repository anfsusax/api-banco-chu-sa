using BankChuSA.Application.Interfaces;
using BankChuSA.Domain.Entities;
using BankChuSA.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace BankChuSA.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BankDbContext _context;
        private IRepository<Account>? _accounts;
        private IRepository<Transaction>? _transactions;
        private IRepository<User>? _users;

        public UnitOfWork(BankDbContext context)
        {
            _context = context;
        }

        public IRepository<Account> Accounts
        {
            get
            {
                _accounts ??= new AccountRepository(_context);
                return _accounts;
            }
        }

        public IRepository<Transaction> Transactions
        {
            get
            {
                _transactions ??= new TransactionRepository(_context);
                return _transactions;
            }
        }

        public IRepository<User> Users
        {
            get
            {
                _users ??= new Repository<User>(_context);
                return _users;
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync()
        {
            // Não inicia transação manual quando há retry strategy
            // A transação será gerenciada pelo execution strategy no CommitTransactionAsync
        }

        public async Task CommitTransactionAsync()
        {
            var strategy = _context.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            });
        }

        public async Task CommitTransactionAsync(Func<Task> operation)
        {
            var strategy = _context.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    await operation();
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            });
        }

        public async Task RollbackTransactionAsync()
        {
            // Rollback é tratado automaticamente pelo execution strategy
            // Se houver uma transação ativa, ela será revertida
            if (_context.Database.CurrentTransaction != null)
            {
                await _context.Database.CurrentTransaction.RollbackAsync();
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}

