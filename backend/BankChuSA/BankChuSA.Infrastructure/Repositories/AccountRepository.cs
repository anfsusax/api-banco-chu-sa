using BankChuSA.Domain.Entities;
using BankChuSA.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BankChuSA.Infrastructure.Repositories
{
    public class AccountRepository : Repository<Account>
    {
        public AccountRepository(BankDbContext context) : base(context)
        {
        }

        public override async Task<Account?> GetByExpressionAsync(System.Linq.Expressions.Expression<Func<Account, bool>> expression)
        {
            return await _dbSet
                .Include(a => a.Transactions)
                .FirstOrDefaultAsync(expression);
        }
    }
}

