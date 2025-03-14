namespace PayphoneWallet.Domain.Entities;

public class Transaction : BaseEntity
{
    public int WalletId { get; set; }
    public decimal Amount { get; set; }
    public TransactionType Type { get; set; }
    public Wallet Wallet { get; set; }
}
