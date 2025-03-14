using AutoMapper;
using Moq;
using PayphoneWallet.Application.Services;
using PayphoneWallet.Domain.DTO;
using PayphoneWallet.Domain.Entities;
using PayphoneWallet.Infrastructure.Interfaces;

namespace PayphoneWallet.Test.Unit.Service;
public class WalletServiceTests
{
    private readonly Mock<IWalletRepository> _mockWalletRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly WalletService _walletService;

    public WalletServiceTests()
    {
        _mockMapper = new Mock<IMapper>();
        _mockWalletRepository = new Mock<IWalletRepository>();
        _walletService = new WalletService(_mockWalletRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task GetWalletByDocumentIdAsync_ExistingWallet_ReturnsWalletDto()
    {
        // Arrange
        string documentId = "11111";
        var wallet = new Wallet
        {
            Id = 1,
            DocumentId = documentId,
            Name = "Test User",
            Balance = 100m
        };
        var walletDto = new WalletDto
        {
            Id = 1,
            DocumentId = documentId,
            Name = "Test User",
            Balance = 100m
        };

        _mockWalletRepository.Setup(repo => repo.GetByDocumentAsync(documentId))
            .ReturnsAsync(wallet);
        _mockMapper.Setup(mapper => mapper.Map<WalletDto>(wallet))
            .Returns(walletDto);

        // Act
        var result = await _walletService.GetWalletByDocumentIdAsync(documentId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(documentId, result.DocumentId);
        Assert.Equal("Test User", result.Name);
        Assert.Equal(100m, result.Balance);
    }

    [Fact]
    public async Task UpdateWalletAsync_NonExistentWallet_ThrowsKeyNotFoundException()
    {
        // Arrange
        var walletDto = new WalletDto
        {
            DocumentId = "null",
            Name = "Test null"
        };

        _mockWalletRepository.Setup(repo => repo.GetByDocumentAsync("null"))
            .ReturnsAsync((Wallet)null);

        // Act and assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _walletService.UpdateWalletAsync(walletDto));
    }

    [Fact]
    public async Task UpdateWalletAsync_FailedUpdate_ThrowsInvalidOperationException()
    {
        // Arrange
        var documentId = "11111";

        var walletDto = new WalletDto
        {
            DocumentId = documentId,
            Name = "Updated Name"
        };
        var existingWallet = new Wallet
        {
            Id = 1,
            DocumentId = documentId,
            Name = "Original Name"
        };

        _mockWalletRepository.Setup(repo => repo.GetByDocumentAsync(documentId))
            .ReturnsAsync(existingWallet);
        _mockWalletRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Wallet>()))
            .ReturnsAsync(false);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _walletService.UpdateWalletAsync(walletDto));
    }

    [Fact]
    public async Task UpdateWalletAsync_ValidUpdate_ReturnsUpdatedWallet()
    {
        // Arrange
        string documentId = "11111";
        var walletDto = new WalletDto
        {
            DocumentId = documentId,
            Name = "new name"
        };
        var existingWallet = new Wallet
        {
            Id = 1,
            DocumentId = documentId,
            Name = "old name",
            Balance = 100m
        };
        var updatedWalletDto = new WalletDto
        {
            Id = 1,
            DocumentId = documentId,
            Name = "new name",
            Balance = 100m
        };

        _mockWalletRepository.Setup(repo => repo.GetByDocumentAsync(documentId))
            .ReturnsAsync(existingWallet);
        _mockWalletRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Wallet>()))
            .ReturnsAsync(true);
        _mockMapper.Setup(mapper => mapper.Map<WalletDto>(It.IsAny<Wallet>()))
            .Returns(updatedWalletDto);

        // Act
        var result = await _walletService.UpdateWalletAsync(walletDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("new name", result.Name);
        Assert.Equal(100m, result.Balance);
    }

    [Fact]
    public async Task DeleteWalletAsync_NonExistentWallet_ReturnsFalse()
    {
        // Arrange
        string documentId = "null";

        _mockWalletRepository.Setup(repo => repo.GetByDocumentAsync(documentId))
            .ReturnsAsync((Wallet)null);

        // Act
        var result = await _walletService.DeleteWalletAsync(documentId);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task DeleteWalletAsync_ExistingWallet_ReturnsRepositoryResult()
    {
        // Arrange
        string documentId = "11111";
        var wallet = new Wallet { Id = 1, DocumentId = documentId };

        _mockWalletRepository.Setup(repo => repo.GetByDocumentAsync(documentId))
            .ReturnsAsync(wallet);
        _mockWalletRepository.Setup(repo => repo.DeleteAsync(1))
            .ReturnsAsync(true);

        // Act
        var result = await _walletService.DeleteWalletAsync(documentId);

        // Assert
        Assert.True(result);
    }
}
