using PayphoneWallet.Domain.Entities;
using PayphoneWallet.Infrastructure.Context;
using PayphoneWallet.Infrastructure.Repositories;
using PayphoneWallet.Test.Fixtures;

namespace PayphoneWallet.Test.Unit.Repository;

public class WalletRepositoryTests : IClassFixture<DatabaseFixture>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly WalletRepository _repository;

    public WalletRepositoryTests(DatabaseFixture fixture)
    {
        _dbContext = fixture.DbContext;
        _repository = new WalletRepository(_dbContext);
    }

    [Fact]
    public async Task GetByDocumentAsync_ExistingDocument_ReturnsWallet()
    {
        // Arrange (11111 exists as walletId = 1)
        string documentId = "11111";

        // Act
        var result = await _repository.GetByDocumentAsync(documentId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(documentId, result.DocumentId);
        Assert.Equal(1, result.Id);
    }

    [Fact]
    public async Task GetByDocumentAsync_NonExistentDocument_ReturnsNull()
    {
        // Arrange (44444 does not exist)
        string documentId = "44444";

        // Act
        var result = await _repository.GetByDocumentAsync(documentId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task AddAsync_NewWallet_SavesCorrectly()
    {
        // Arrange
        var wallet = new Wallet
        {
            DocumentId = "66666",
            Name = "Test6",
            Balance = 100m,
            CreatedAt = DateTime.UtcNow
        };

        // Act
        await _repository.AddAsync(wallet);
        var savedWallet = await _repository.GetByDocumentAsync("66666");

        // Assert
        Assert.NotNull(savedWallet);
        Assert.Equal(wallet.DocumentId, savedWallet.DocumentId);
        Assert.Equal(wallet.Name, savedWallet.Name);
        Assert.Equal(wallet.Balance, savedWallet.Balance);
    }

    [Fact]
    public async Task UpdateAsync_ExistingWallet_UpdatesCorrectly()
    {
        // Arrange
        var wallet = await _repository.GetByDocumentAsync("11111");
        Assert.NotNull(wallet);

        string updatedName = "Test1New";
        wallet.Name = updatedName;

        // Act
        await _repository.UpdateAsync(wallet);
        var updatedWallet = await _repository.GetByDocumentAsync("11111");

        // Assert
        Assert.NotNull(updatedWallet);
        Assert.Equal(updatedName, updatedWallet.Name);
    }

    [Fact]
    public async Task DeleteAsync_ExistingWallet_DeletesCorrectly()
    {
        // Arrange
        var wallet = await _repository.GetByDocumentAsync("22222");
        Assert.NotNull(wallet);

        // Act
        await _repository.DeleteAsync(wallet.Id);
        var deletedWallet = await _repository.GetByDocumentAsync("22222");

        // Assert
        Assert.Null(deletedWallet);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllWallets()
    {
        // Act
        var wallets = await _repository.GetAllAsync();

        // Assert
        Assert.NotNull(wallets);
        Assert.NotEmpty(wallets);
    }

    [Fact]
    public async Task GetByIdAsync_ExistingId_ReturnsWallet()
    {
        // Arrange
        int walletId = 1; // Assuming wallet with ID 1 exists

        // Act
        var wallet = await _repository.GetByIdAsync(walletId);

        // Assert
        Assert.NotNull(wallet);
        Assert.Equal(walletId, wallet.Id);
    }

    [Fact]
    public async Task GetByIdAsync_NonExistentId_ReturnsNull()
    {
        // Arrange
        int nonExistentId = 999;

        // Act
        var wallet = await _repository.GetByIdAsync(nonExistentId);

        // Assert
        Assert.Null(wallet);
    }
}
