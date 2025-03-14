using System.ComponentModel.DataAnnotations;

namespace PayphoneWallet.Domain.DTO;

public class TransactionDto
{
    public int Id { get; set; }

    [Required]
    public int WalletId { get; set; }

    [Required]
    public int DestinationWalledId { get; set; }

    [Required]
    [Range(double.Epsilon, double.MaxValue)]
    public decimal Amount { get; set; }

    [Required]
    public string Type { get; set; }

    public DateTime CreatedAt { get; set; }
}
