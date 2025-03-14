using AutoMapper;
using Moq;
using PayphoneWallet.Application.Services;
using PayphoneWallet.Domain.DTO;
using PayphoneWallet.Domain.Entities;
using PayphoneWallet.Infrastructure.Interfaces;

namespace PayphoneWallet.Test.Unit.Service;

public class TransactionServiceTest
{
    private readonly Mock<ITransactionRepository> _mockTransactionRepository;
    private readonly Mock<IWalletRepository> _mockWalletRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly TransactionService _transactionService;

    public TransactionServiceTest()
    {
        _mockTransactionRepository = new Mock<ITransactionRepository>();
        _mockWalletRepository = new Mock<IWalletRepository>();
        _mockMapper = new Mock<IMapper>();

        _transactionService = new TransactionService(_mockTransactionRepository.Object, _mockMapper.Object, _mockWalletRepository.Object);
    }

    [Fact]
    public async Task GetTransactionsByDocumentId_ExistingWallet_ReturnsTransactionDtoList()
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
        var transactions = new List<Transaction>
        {
            new() {
                Id = 1,
                WalletId = wallet.Id,
                DestinationWalletId = 2,
                Amount = 50m,
                CreatedAt = DateTime.UtcNow
            },
            new() {
                Id = 2,
                WalletId = wallet.Id,
                DestinationWalletId = 3,
                Amount = 25m,
                CreatedAt = DateTime.UtcNow
            }
        };
        var transactionDtos = new List<TransactionDto>
        {
            new() {
                Id = 1,
                WalletId = wallet.Id,
                DestinationWalletId = 2,
                Amount = 50m,
                CreatedAt = DateTime.UtcNow
            },
            new() {
                Id = 2,
                WalletId = wallet.Id,
                DestinationWalletId = 3,
                Amount = 25m,
                CreatedAt = DateTime.UtcNow
            }
        };

        // Act
        _mockWalletRepository.Setup(repo => repo.GetByDocumentAsync(documentId))
            .ReturnsAsync(wallet);
        _mockTransactionRepository.Setup(repo => repo.GetByWalletIdAsync(wallet.Id))
            .ReturnsAsync(transactions);
        _mockMapper.Setup(mapper => mapper.Map<List<TransactionDto>>(transactions))
            .Returns(transactionDtos);

        // Assert
        var result = await _transactionService.GetTransactionsByDocumentIdAsync(documentId);
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Equal(50m, result[0].Amount);
        Assert.Equal(25m, result[1].Amount);
    }

    [Fact]
    public async Task GetTransactionsByDocumentId_NonExistentWallet_ThrowsKeyNotFoundException()
    {
        // Arrange
        string documentId = "11111";
        var walletDto = new WalletDto
        {
            Id = 1,
            DocumentId = documentId,
            Name = "Test User",
            Balance = 100m
        };

        _mockWalletRepository.Setup(repo => repo.GetByDocumentAsync(walletDto.DocumentId))
            .ReturnsAsync((Wallet)null);

        // Act and assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _transactionService.GetTransactionsByDocumentIdAsync(walletDto.DocumentId));
    }
}
