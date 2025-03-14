using Microsoft.EntityFrameworkCore;
using PayphoneWallet.Domain.Entities;
using PayphoneWallet.Infrastructure.Context;

namespace PayphoneWallet.Test.Fixtures;

public class DatabaseFixture : IDisposable
{
    public ApplicationDbContext DbContext { get; }

    public DatabaseFixture()
    {
        // Create test memory db
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlite("DataSource=:memory:")
            .Options;

        // Instance dbcontext and ensure stuff
        DbContext = new ApplicationDbContext(options);
        DbContext.Database.OpenConnection();
        DbContext.Database.EnsureCreated();

        // Create seed data
        var wallets = new List<Wallet>
        {
            new() {
                Id = 1,
                DocumentId = "11111",
                Name = "Test1",
                Balance = 1000m,
            },
            new() {
                Id = 2,
                DocumentId = "22222",
                Name = "Test2",
                Balance = 500m,
            },
            new() {
                Id = 3,
                DocumentId = "33333",
                Name = "Test3",
                Balance = 5000m,
            }
        };
        DbContext.Wallets.AddRange(wallets);

        // Seed test transactions
        var transactions = new List<Transaction>
        {
            new() {
                WalletId = 1,
                DestinationWalletId = 2,
                Amount = 100m,
                Type = TransactionType.Credit,
            },
            new() {
                WalletId = 2,
                DestinationWalletId = 1,
                Amount = 200m,
                Type = TransactionType.Debit,
            }
        };
        DbContext.Transactions.AddRange(transactions);

        DbContext.SaveChanges();
    }

    public void Dispose()
    {
        DbContext.Database.CloseConnection();
        DbContext.Dispose();
    }
}