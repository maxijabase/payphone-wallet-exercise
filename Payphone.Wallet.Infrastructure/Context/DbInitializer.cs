using PayphoneWallet.Domain.Entities;

namespace PayphoneWallet.Infrastructure.Context;

public class DbInitializer
{
    private readonly ApplicationDbContext _context;

    public DbInitializer(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Initialize()
    {
        try
        {
            if (!_context.Wallets.Any())
            {
                var wallets = new List<Wallet>
                {
                    new() {
                        DocumentId = "42000000",
                        Name = "Maxi Jabase",
                        Balance = 10000m,
                    },
                    new() {
                        DocumentId = "43000000",
                        Name = "Mauri Jabase",
                        Balance = 5000m,
                    },
                    new() {
                        DocumentId = "44000000",
                        Name = "Guille Jabase",
                        Balance = 5000m,
                    }
                };

                _context.Wallets.AddRange(wallets);
                await _context.SaveChangesAsync();
            }
        }
        catch
        {
            throw;
        }
    }
}
