using AutoMapper;
using PayphoneWallet.Application.Interfaces;
using PayphoneWallet.Domain.DTO;
using PayphoneWallet.Domain.Entities;
using PayphoneWallet.Infrastructure.Interfaces;

namespace PayphoneWallet.Application.Services;

public class WalletService : IWalletService
{
    private readonly IWalletRepository _walletRepository;
    private readonly IMapper _mapper;

    public WalletService(IWalletRepository walletRepository, IMapper mapper)
    {
        _walletRepository = walletRepository;
        _mapper = mapper;
    }

    public async Task<WalletDto> GetWalletByDocumentIdAsync(string document)
    {
        var result = await _walletRepository.GetByDocumentAsync(document);
        return _mapper.Map<WalletDto>(result);
    }

    public async Task<WalletDto> CreateWalletAsync(WalletDto wallet)
    {
        await _walletRepository.AddAsync(_mapper.Map<Wallet>(wallet));
        return wallet;
    }

    public async Task<WalletDto> UpdateWalletAsync(WalletDto walletDto)
    {
        var existingWallet = await _walletRepository.GetByDocumentAsync(walletDto.DocumentId);
        if (existingWallet == null)
        {
            throw new KeyNotFoundException($"Wallet with ID {walletDto.DocumentId} not found");
        }

        // only name is allowed to change
        existingWallet.Name = walletDto.Name;

        var success = await _walletRepository.UpdateAsync(existingWallet);
        if (!success)
        {
            throw new InvalidOperationException("Failed to update wallet");
        }

        return _mapper.Map<WalletDto>(existingWallet);
    }

    public async Task<bool> DeleteWalletAsync(string id)
    {
        var wallet = await _walletRepository.GetByDocumentAsync(id);
        if (wallet == null)
        {
            return false;
        }
        return await _walletRepository.DeleteAsync(wallet.Id);
    }
}
