namespace PayphoneWallet.Domain.Entities;

public class Wallet : BaseEntity
{
    public string DocumentId { get; set; }
    public string Name { get; set; }
    public decimal Balance { get; set; }
    public ICollection<Transaction> Transactions { get; set; } = [];
}
