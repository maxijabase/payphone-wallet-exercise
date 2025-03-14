using Microsoft.EntityFrameworkCore;
using PayphoneWallet.Domain.Entities;
using PayphoneWallet.Infrastructure.Context;
using PayphoneWallet.Infrastructure.Interfaces;

namespace PayphoneWallet.Infrastructure.Repositories;

public class WalletRepository : GenericRepository<Wallet>, IWalletRepository
{
    public WalletRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Wallet> GetByDocumentAsync(string document)
    {
        return await _dbSet.FirstOrDefaultAsync(x => x.DocumentId == document);
    }
}
