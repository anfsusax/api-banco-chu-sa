using System.Globalization;
using System.Resources;

namespace BankChuSA.Application.Resources
{
    public static class Messages
    {
        private static readonly ResourceManager ResourceManager =
            new ResourceManager("BankChuSA.Application.Resources.Messages", typeof(Messages).Assembly);

        private static string GetString(string name) =>
            ResourceManager.GetString(name, CultureInfo.CurrentUICulture) ?? 
            ResourceManager.GetString(name, new CultureInfo("pt-BR")) ?? 
            name;

        public static string TransferSuccess => GetString("TransferSuccess");
        public static string AdminUserCreated => GetString("AdminUserCreated");
        public static string InvalidCredentials => GetString("InvalidCredentials");
    }
}
