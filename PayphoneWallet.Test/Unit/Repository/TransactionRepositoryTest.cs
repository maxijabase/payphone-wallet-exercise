using PayphoneWallet.Domain.Entities;
using PayphoneWallet.Infrastructure.Context;
using PayphoneWallet.Infrastructure.Repositories;
using PayphoneWallet.Test.Fixtures;

namespace PayphoneWallet.Test.Unit.Repository;

public class TransactionRepositoryTests : IClassFixture<DatabaseFixture>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly TransactionRepository _repository;

    public TransactionRepositoryTests(DatabaseFixture fixture)
    {
        _dbContext = fixture.DbContext;
        _repository = new TransactionRepository(_dbContext);
    }

    [Fact]
    public async Task GetByWalletIdAsync_ExistingWalletWithTransactions_ReturnsTransactions()
    {
        // Arrange
        int walletId = 1;

        // Act
        var result = await _repository.GetByWalletIdAsync(walletId);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.All(result, item => Assert.Equal(walletId, item.WalletId));
    }

    [Fact]
    public async Task GetByWalletIdAsync_ExistingWalletWithNoTransactions_ReturnsEmptyList()
    {
        // Arrange (wallet 3 was created with no transactions associated)
        int walletId = 3;

        // Act
        var result = await _repository.GetByWalletIdAsync(walletId);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetByWalletIdAsync_NonExistentWallet_ReturnsEmptyList()
    {
        // Arrange (4 does not exist)
        int nonExistentWalletId = 4;

        // Act
        var result = await _repository.GetByWalletIdAsync(nonExistentWalletId);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task AddAsync_NewTransaction_SavesCorrectly()
    {
        // Arrange
        var transaction = new Transaction
        {
            WalletId = 1,
            Amount = 50m,
            Type = TransactionType.Debit,
        };

        // Act
        await _repository.AddAsync(transaction);
        var savedTransaction = await _dbContext.Transactions.FindAsync(transaction.Id);

        // Assert
        Assert.NotNull(savedTransaction);
        Assert.Equal(transaction.WalletId, savedTransaction.WalletId);
        Assert.Equal(transaction.Amount, savedTransaction.Amount);
        Assert.Equal(transaction.Type, savedTransaction.Type);
    }
}
