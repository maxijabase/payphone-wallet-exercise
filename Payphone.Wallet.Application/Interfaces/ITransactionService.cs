using PayphoneWallet.Domain.DTO;

namespace PayphoneWallet.Application.Interfaces;

public interface ITransactionService
{
    Task<List<TransactionDto>> GetTransactionsByDocumentIdAsync(string documentId);
    Task<TransactionDto> CreateTransactionAsync(TransactionDto transaction);
}
