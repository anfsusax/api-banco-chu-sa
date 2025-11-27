using BankChuSA.Application.DTOs;
using BankChuSA.Application.Interfaces;
using BankChuSA.Application.Services;
using BankChuSA.Domain.Entities;
using BankChuSA.Infrastructure.Data;
using BankChuSA.Infrastructure.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace BankChuSA.Tests.Services
{
    public class AccountServiceTests
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AccountService _accountService;
        private readonly BankDbContext _context;

        public AccountServiceTests()
        {
            var options = new DbContextOptionsBuilder<BankDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new BankDbContext(options);
            _unitOfWork = new UnitOfWork(_context);
            _accountService = new AccountService(_unitOfWork);
        }

        [Fact]
        public async Task CreateAccountAsync_ShouldCreateAccount_WhenValidData()
        {
            // Arrange
            var createAccountDto = new CreateAccountDto
            {
                OwnerName = "João Silva",
                DocumentNumber = "12345678901",
                InitialBalance = 1000.00m
            };

            // Act
            var result = await _accountService.CreateAccountAsync(createAccountDto);

            // Assert
            result.Should().NotBeNull();
            result.AccountNumber.Should().NotBeNullOrEmpty();
            result.OwnerName.Should().Be(createAccountDto.OwnerName);
            result.DocumentNumber.Should().Be(createAccountDto.DocumentNumber);
            result.Balance.Should().Be(createAccountDto.InitialBalance);
        }

        [Fact]
        public async Task GetAccountByNumberAsync_ShouldReturnAccount_WhenAccountExists()
        {
            // Arrange
            var account = new Account
            {
                Id = Guid.NewGuid(),
                AccountNumber = "123456",
                OwnerName = "João Silva",
                DocumentNumber = "12345678901",
                Balance = 1000.00m,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            await _unitOfWork.Accounts.AddAsync(account);
            await _unitOfWork.SaveChangesAsync();

            // Act
            var result = await _accountService.GetAccountByNumberAsync("123456");

            // Assert
            result.Should().NotBeNull();
            result!.AccountNumber.Should().Be("123456");
            result.OwnerName.Should().Be("João Silva");
        }

        [Fact]
        public async Task GetAccountByNumberAsync_ShouldReturnNull_WhenAccountDoesNotExist()
        {
            // Act
            var result = await _accountService.GetAccountByNumberAsync("999999");

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task AccountExistsAsync_ShouldReturnTrue_WhenAccountExists()
        {
            // Arrange
            var account = new Account
            {
                Id = Guid.NewGuid(),
                AccountNumber = "123456",
                OwnerName = "João Silva",
                DocumentNumber = "12345678901",
                Balance = 1000.00m,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            await _unitOfWork.Accounts.AddAsync(account);
            await _unitOfWork.SaveChangesAsync();

            // Act
            var result = await _accountService.AccountExistsAsync("123456");

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task AccountExistsAsync_ShouldReturnFalse_WhenAccountDoesNotExist()
        {
            // Act
            var result = await _accountService.AccountExistsAsync("999999");

            // Assert
            result.Should().BeFalse();
        }
    }
}

