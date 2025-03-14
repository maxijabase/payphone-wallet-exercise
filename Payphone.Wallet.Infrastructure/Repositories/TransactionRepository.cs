using Microsoft.EntityFrameworkCore;
using PayphoneWallet.Domain.Entities;
using PayphoneWallet.Infrastructure.Context;
using PayphoneWallet.Infrastructure.Interfaces;

namespace PayphoneWallet.Infrastructure.Repositories;

public class TransactionRepository : GenericRepository<Transaction>, ITransactionRepository
{
    public TransactionRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<List<Transaction>> GetByWalletIdAsync(int walletId)
    {
        return await _dbSet.Where(x => x.WalletId == walletId).ToListAsync();
    }
}
