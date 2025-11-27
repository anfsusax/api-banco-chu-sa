using BankChuSA.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BankChuSA.Infrastructure.Data
{
    public class BankDbContext : DbContext
    {
        public BankDbContext(DbContextOptions<BankDbContext> options) : base(options)
        {
        }

        public DbSet<Account> Accounts { get; set; } = null!;
        public DbSet<Transaction> Transactions { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.AccountNumber).IsUnique();
                entity.Property(e => e.AccountNumber).HasMaxLength(20).IsRequired();
                entity.Property(e => e.OwnerName).HasMaxLength(200).IsRequired();
                entity.Property(e => e.DocumentNumber).HasMaxLength(14).IsRequired();
                entity.Property(e => e.Balance).HasPrecision(18, 2);

                entity.HasMany(e => e.Transactions)
                    .WithOne(e => e.Account)
                    .HasForeignKey(e => e.AccountId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Amount).HasPrecision(18, 2);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.TransactionDate).IsRequired();

                entity.HasOne(e => e.RelatedAccount)
                    .WithMany()
                    .HasForeignKey(e => e.RelatedAccountId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Username).IsUnique();
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.Username).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Email).HasMaxLength(200).IsRequired();
                entity.Property(e => e.Role).HasMaxLength(50).IsRequired();
            });
        }
    }

}