using Microsoft.AspNetCore.Mvc;
using PayphoneWallet.API.Filters;
using PayphoneWallet.Application.Interfaces;
using PayphoneWallet.Domain.DTO;

namespace PayphoneWallet.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[ValidateModel]
public class TransactionController : ControllerBase
{
    private readonly ITransactionService _transactionService;

    public TransactionController(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    // POST: transaction
    [HttpPost]
    public async Task<IActionResult> CreateTransaction([FromBody] TransactionDto transaction)
    {
        var result = await _transactionService.CreateTransactionAsync(transaction);
        return CreatedAtAction("GetTransaction", new { id = result.Id }, result);
    }
}
