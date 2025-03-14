using PayphoneWallet.Domain.Entities;

namespace PayphoneWallet.Infrastructure.Interfaces;

public interface ITransactionRepository : IGenericRepository<Transaction>
{
    Task<List<Transaction>> GetByWalletIdAsync(int walletId);
}
