using System.Globalization;
using System.Resources;

namespace BankChuSA.Application.Resources
{
    public static class ErrorMessages
    {
        private static readonly ResourceManager ResourceManager =
            new ResourceManager("BankChuSA.Application.Resources.ErrorMessages", typeof(ErrorMessages).Assembly);

        private static string GetString(string name) =>
            ResourceManager.GetString(name, CultureInfo.CurrentUICulture) ?? 
            ResourceManager.GetString(name, new CultureInfo("pt-BR")) ?? 
            name;

        public static string JwtKeyNotConfigured => GetString("JwtKeyNotConfigured");

        public static string TransferOnlyOnWorkingDays => GetString("TransferOnlyOnWorkingDays");
        public static string SourceAccountNotFound => GetString("SourceAccountNotFound");
        public static string DestinationAccountNotFound => GetString("DestinationAccountNotFound");
        public static string InsufficientBalance => GetString("InsufficientBalance");
        public static string AccountNotFound => GetString("AccountNotFound");
        public static string MigrationError => GetString("MigrationError");
        public static string FromAccountRequired => GetString("FromAccountRequired");
        public static string ToAccountRequired => GetString("ToAccountRequired");
        public static string AccountsCannotBeEqual => GetString("AccountsCannotBeEqual");
        public static string TransferAmountMustBeGreaterThanZero => GetString("TransferAmountMustBeGreaterThanZero");
        public static string DescriptionMaxLength => GetString("DescriptionMaxLength");
        public static string OwnerNameRequired => GetString("OwnerNameRequired");
        public static string OwnerNameMaxLength => GetString("OwnerNameMaxLength");
        public static string DocumentNumberRequired => GetString("DocumentNumberRequired");
        public static string DocumentNumberLength => GetString("DocumentNumberLength");
        public static string InitialBalanceCannotBeNegative => GetString("InitialBalanceCannotBeNegative");
    }
}