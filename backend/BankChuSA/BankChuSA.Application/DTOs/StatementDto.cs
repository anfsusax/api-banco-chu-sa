namespace BankChuSA.Application.DTOs
{
    public class StatementDto
    {
        public Guid AccountId { get; set; }
        public string AccountNumber { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal InitialBalance { get; set; }
        public decimal FinalBalance { get; set; }
        public List<TransactionItemDto> Transactions { get; set; } = new();
    }
}

