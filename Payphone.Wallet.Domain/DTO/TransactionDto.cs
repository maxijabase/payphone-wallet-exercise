﻿namespace PayphoneWallet.Domain.DTO;

public class TransactionDto
{
    public int Id { get; set; }
    public int WalletId { get; set; }
    public decimal Amount { get; set; }
    public string Type { get; set; }
    public DateTime CreatedAt { get; set; }
}
