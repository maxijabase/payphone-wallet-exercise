using System.ComponentModel.DataAnnotations;

namespace PayphoneWallet.Domain.DTO;

public class WalletDto
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string DocumentId { get; set; }
    public decimal Balance { get; set; }
}
