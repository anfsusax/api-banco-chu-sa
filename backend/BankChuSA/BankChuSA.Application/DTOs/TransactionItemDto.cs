namespace BankChuSA.Application.DTOs
{
    public class TransactionItemDto
    {
        public DateTime TransactionDate { get; set; }
        public string Type { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Description { get; set; } = string.Empty;
        public string? RelatedAccountNumber { get; set; }
    }
}
