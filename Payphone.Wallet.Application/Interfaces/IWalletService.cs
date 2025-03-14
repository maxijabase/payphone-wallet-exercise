using PayphoneWallet.Domain.DTO;

namespace PayphoneWallet.Application.Interfaces;

public interface IWalletService
{
    Task<WalletDto> GetWalletByDocumentIdAsync(string document);
    Task<WalletDto> CreateWalletAsync(WalletDto wallet);
    Task<WalletDto> UpdateWalletAsync(WalletDto wallet);
    Task<bool> DeleteWalletAsync(string id);
}
