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

    /// <summary>
    /// Creates a new transaction
    /// </summary>
    /// <param name="transaction">The transaction data</param>
    /// <returns>The newly created transaction</returns>
    /// <response code="201">Returns the newly created transaction</response>
    /// <response code="400">If the transaction data is invalid</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPost]
    public async Task<IActionResult> CreateTransaction([FromBody] TransactionDto transaction)
    {
        var result = await _transactionService.CreateTransactionAsync(transaction);
        return Ok(result);
    }
}
