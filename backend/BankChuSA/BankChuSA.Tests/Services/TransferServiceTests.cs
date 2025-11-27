using BankChuSA.Application.DTOs;
using BankChuSA.Application.Interfaces;
using BankChuSA.Application.Services;
using BankChuSA.Domain.Entities;
using BankChuSA.Domain.Enums;
using BankChuSA.Infrastructure.Data;
using BankChuSA.Infrastructure.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace BankChuSA.Tests.Services
{
    public class TransferServiceTests
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly Mock<IHolidayService> _holidayServiceMock;
        private readonly TransferService _transferService;
        private readonly BankDbContext _context;

        public TransferServiceTests()
        {
            var options = new DbContextOptionsBuilder<BankDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new BankDbContext(options);
            _unitOfWork = new UnitOfWork(_context);
            _holidayServiceMock = new Mock<IHolidayService>();
            _transferService = new TransferService(_unitOfWork, _holidayServiceMock.Object);
        }

        [Fact]
        public async Task ExecuteTransferAsync_ShouldTransferSuccessfully_WhenValidData()
        {
            // Arrange
            _holidayServiceMock.Setup(x => x.IsWorkingDayAsync(It.IsAny<DateTime>()))
                .ReturnsAsync(true);

            var fromAccount = new Account
            {
                Id = Guid.NewGuid(),
                AccountNumber = "111111",
                OwnerName = "João Silva",
                DocumentNumber = "12345678901",
                Balance = 1000.00m,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            var toAccount = new Account
            {
                Id = Guid.NewGuid(),
                AccountNumber = "222222",
                OwnerName = "Maria Santos",
                DocumentNumber = "98765432100",
                Balance = 500.00m,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            await _unitOfWork.Accounts.AddAsync(fromAccount);
            await _unitOfWork.Accounts.AddAsync(toAccount);
            await _unitOfWork.SaveChangesAsync();

            var transferDto = new TransferDto
            {
                FromAccountNumber = "111111",
                ToAccountNumber = "222222",
                Amount = 200.00m,
                Description = "Transferência teste"
            };

            // Act
            var result = await _transferService.ExecuteTransferAsync(transferDto);

            // Assert
            result.Should().BeTrue();

            var updatedFromAccount = await _unitOfWork.Accounts.GetByExpressionAsync(a => a.AccountNumber == "111111");
            var updatedToAccount = await _unitOfWork.Accounts.GetByExpressionAsync(a => a.AccountNumber == "222222");

            updatedFromAccount!.Balance.Should().Be(800.00m);
            updatedToAccount!.Balance.Should().Be(700.00m);
        }

        [Fact]
        public async Task ExecuteTransferAsync_ShouldThrowException_WhenNotWorkingDay()
        {
            // Arrange
            _holidayServiceMock.Setup(x => x.IsWorkingDayAsync(It.IsAny<DateTime>()))
                .ReturnsAsync(false);

            var transferDto = new TransferDto
            {
                FromAccountNumber = "111111",
                ToAccountNumber = "222222",
                Amount = 200.00m,
                Description = "Transferência teste"
            };

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(
                () => _transferService.ExecuteTransferAsync(transferDto));
        }

        [Fact]
        public async Task ExecuteTransferAsync_ShouldThrowException_WhenInsufficientBalance()
        {
            // Arrange
            _holidayServiceMock.Setup(x => x.IsWorkingDayAsync(It.IsAny<DateTime>()))
                .ReturnsAsync(true);

            var fromAccount = new Account
            {
                Id = Guid.NewGuid(),
                AccountNumber = "111111",
                OwnerName = "João Silva",
                DocumentNumber = "12345678901",
                Balance = 100.00m,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            var toAccount = new Account
            {
                Id = Guid.NewGuid(),
                AccountNumber = "222222",
                OwnerName = "Maria Santos",
                DocumentNumber = "98765432100",
                Balance = 500.00m,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            await _unitOfWork.Accounts.AddAsync(fromAccount);
            await _unitOfWork.Accounts.AddAsync(toAccount);
            await _unitOfWork.SaveChangesAsync();

            var transferDto = new TransferDto
            {
                FromAccountNumber = "111111",
                ToAccountNumber = "222222",
                Amount = 200.00m,
                Description = "Transferência teste"
            };

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(
                () => _transferService.ExecuteTransferAsync(transferDto));
        }
    }
}

