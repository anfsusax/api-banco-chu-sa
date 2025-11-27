namespace BankChuSA.Application.DTOs
{
    public class CreateAccountDto
    {
        public string OwnerName { get; set; } = string.Empty;
        public string DocumentNumber { get; set; } = string.Empty;
        public decimal InitialBalance { get; set; }
    }
}