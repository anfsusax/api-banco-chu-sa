using BankChuSA.Domain.Enums;

namespace BankChuSA.Domain.Entities
{
    public class Transaction
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public TransactionType Type { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime TransactionDate { get; set; }
        public Guid? RelatedAccountId { get; set; }

        public virtual Account Account { get; set; } = null!;
        public virtual Account? RelatedAccount { get; set; }
    }
}
