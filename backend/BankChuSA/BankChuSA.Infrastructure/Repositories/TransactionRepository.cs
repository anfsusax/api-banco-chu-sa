using BankChuSA.Domain.Entities;
using BankChuSA.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace BankChuSA.Infrastructure.Repositories
{
    public class TransactionRepository : Repository<Transaction>
    {
        public TransactionRepository(BankDbContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<Transaction>> FindAsync(System.Linq.Expressions.Expression<Func<Transaction, bool>> expression)
        {
            return await _dbSet
                .Include(t => t.RelatedAccount)
                .Where(expression)
                .ToListAsync();
        }

    }
}

