using PayphoneWallet.Domain.Entities;

namespace PayphoneWallet.Infrastructure.Interfaces;

public interface IWalletRepository : IGenericRepository<Wallet>
{
    Task<Wallet?> GetByDocumentAsync(string document);
}
