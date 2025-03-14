namespace PayphoneWallet.Domain.DTO;

public class WalletDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string DocumentId { get; set; }
    public decimal Balance { get; set; }
}
