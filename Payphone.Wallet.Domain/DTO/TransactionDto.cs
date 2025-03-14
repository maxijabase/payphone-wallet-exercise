using System.ComponentModel.DataAnnotations;

namespace PayphoneWallet.Domain.DTO;

public class TransactionDto
{
    public int Id { get; set; }

    [Required]
    public int WalletId { get; set; }

    [Required]
    public int DestinationWalletId { get; set; }

    [Required]
    [Range(0, (double)decimal.MaxValue, ErrorMessage = "Amount must be greater than 0!")]
    public decimal Amount { get; set; }

    [Required]
    public string Type { get; set; }

    public DateTime CreatedAt { get; set; }
}
