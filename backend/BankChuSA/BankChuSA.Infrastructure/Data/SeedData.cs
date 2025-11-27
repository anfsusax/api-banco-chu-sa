using BankChuSA.Application.Services;
using BankChuSA.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BankChuSA.Infrastructure.Data
{
    public static class SeedData
    {
        public static async Task SeedAsync(BankDbContext context)
        {
            var adminUser = await context.Users.FirstOrDefaultAsync(u => u.Username == "admin");

            if (adminUser == null)
            {
                var user = new User
                {
                    Id = Guid.NewGuid(),
                    Username = "admin",
                    Email = "admin@bankchusa.com",
                    PasswordHash = AuthService.HashPassword("admin123"),
                    Role = "Admin",
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true
                };

                context.Users.Add(user);
                await context.SaveChangesAsync();
            }
            else if (!adminUser.IsActive || adminUser.PasswordHash == null)
            {
                adminUser.PasswordHash = AuthService.HashPassword("admin123");
                adminUser.IsActive = true;
                adminUser.Role = "Admin";
                await context.SaveChangesAsync();
            }
        }
    }
}