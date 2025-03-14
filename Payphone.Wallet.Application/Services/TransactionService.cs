using AutoMapper;
using PayphoneWallet.Application.Interfaces;
using PayphoneWallet.Domain.DTO;
using PayphoneWallet.Domain.Entities;
using PayphoneWallet.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayphoneWallet.Application.Services;

public class TransactionService : ITransactionService
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IWalletRepository _walletRepository;
    private readonly IMapper _mapper;

    public TransactionService(ITransactionRepository transactionRepository, IMapper mapper, IWalletRepository walletRepository)
    {
        _transactionRepository = transactionRepository;
        _mapper = mapper;
        _walletRepository = walletRepository;
    }

    public async Task<List<TransactionDto>> GetTransactionsByDocumentIdAsync(string documentId)
    {
        var wallet = await _walletRepository.GetByDocumentAsync(documentId);
        if (wallet == null)
        {
            throw new KeyNotFoundException($"Wallet with ID {documentId} not found");
        }
        var result = await _transactionRepository.GetByWalletIdAsync(wallet.Id);
        return _mapper.Map<List<TransactionDto>>(result);
    }

    public async Task<TransactionDto> CreateTransactionAsync(TransactionDto transactionDto)
    {
        if (transactionDto.Amount <= 0)
        {
            throw new ArgumentException("Transaction amount must be greater than zero");
        }

        var wallet = await _walletRepository.GetByIdAsync(transactionDto.WalletId);
        if (wallet == null)
        {
            throw new KeyNotFoundException($"Wallet with ID {transactionDto.WalletId} not found");
        }

        if (wallet.Balance < transactionDto.Amount)
        {
            throw new InvalidOperationException("Insufficient balance for this transaction");
        }

        var transaction = _mapper.Map<Transaction>(transactionDto);
        transaction.CreatedAt = DateTime.UtcNow;

        wallet.Balance -= transaction.Amount;

        await _transactionRepository.AddAsync(transaction);
        await _walletRepository.UpdateAsync(wallet);

        return _mapper.Map<TransactionDto>(transaction);
    }
}
