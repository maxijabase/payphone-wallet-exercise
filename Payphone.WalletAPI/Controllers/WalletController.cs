using Microsoft.AspNetCore.Mvc;
using PayphoneWallet.API.Filters;
using PayphoneWallet.Application.Interfaces;
using PayphoneWallet.Domain.DTO;

namespace PayphoneWallet.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[ValidateModel]
public class WalletController : ControllerBase
{
    private readonly IWalletService _walletService;
    private readonly ITransactionService _transactionService;

    public WalletController(IWalletService walletService, ITransactionService transactionService)
    {
        _walletService = walletService;
        _transactionService = transactionService;
    }

    /// <summary>
    /// Gets a wallet by document ID
    /// </summary>
    /// <param name="id">The document ID of the wallet owner</param>
    /// <returns>The wallet information</returns>
    /// <response code="200">Returns the wallet information</response>
    /// <response code="404">If the wallet is not found</response>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetWalletByDocument(string id)
    {
        var wallet = await _walletService.GetWalletByDocumentIdAsync(id);
        if (wallet == null)
        {
            return NotFound();
        }
        return Ok(wallet);
    }

    /// <summary>
    /// Gets all transactions for a specified wallet
    /// </summary>
    /// <param name="id">The document ID of the wallet owner</param>
    /// <returns>A list of transactions</returns>
    /// <response code="200">Returns the list of transactions</response>
    [HttpGet("{id}/transactions")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetWalletTransactions(string id)
    {
        var transactions = await _transactionService.GetTransactionsByDocumentIdAsync(id);
        return Ok(transactions);
    }

    /// <summary>
    /// Creates a new wallet
    /// </summary>
    /// <param name="wallet">The wallet data</param>
    /// <returns>The newly created wallet</returns>
    /// <response code="201">Returns the newly created wallet</response>
    /// <response code="400">If the wallet data is invalid</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateWallet([FromBody] WalletDto wallet)
    {
        var result = await _walletService.CreateWalletAsync(wallet);
        return CreatedAtAction("GetWalletByDocument", new { id = result.DocumentId }, result);
    }

    /// <summary>
    /// Updates an existing wallet
    /// </summary>
    /// <param name="id">The document ID of the wallet owner</param>
    /// <param name="wallet">The updated wallet data</param>
    /// <returns>The updated wallet</returns>
    /// <response code="200">Returns the updated wallet</response>
    /// <response code="400">If the ID does not match the wallet document ID</response>
    /// <response code="404">If the wallet is not found</response>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateWallet(string id, [FromBody] WalletDto wallet)
    {
        if (id != wallet.DocumentId)
        {
            return BadRequest();
        }
        var result = await _walletService.UpdateWalletAsync(wallet);
        if (result == null)
        {
            return NotFound();
        }
        return Ok(result);
    }

    /// <summary>
    /// Deletes a wallet
    /// </summary>
    /// <param name="id">The document ID of the wallet owner</param>
    /// <returns>No content</returns>
    /// <response code="204">If the wallet was successfully deleted</response>
    /// <response code="404">If the wallet is not found</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteWallet(string id)
    {
        var result = await _walletService.DeleteWalletAsync(id);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }
}
