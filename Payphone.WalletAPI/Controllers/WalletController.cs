using Microsoft.AspNetCore.Mvc;
using PayphoneWallet.Application.Interfaces;
using PayphoneWallet.Domain.DTO;

namespace PayphoneWallet.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WalletController : ControllerBase
{
    private readonly IWalletService _walletService;
    private readonly ITransactionService _transactionService;

    public WalletController(IWalletService walletService, ITransactionService transactionService)
    {
        _walletService = walletService;
        _transactionService = transactionService;
    }

    // GET wallet/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetWalletByDocument(string id)
    {
        var wallet = await _walletService.GetWalletByDocumentIdAsync(id);
        if (wallet == null)
        {
            return NotFound();
        }
        return Ok(wallet);
    }

    // GET: wallet/5/transactions
    [HttpGet("{id}/transactions")]
    public async Task<IActionResult> GetWalletTransactions(string id)
    {
        var transactions = await _transactionService.GetTransactionsByDocumentIdAsync(id);
        return Ok(transactions);
    }

    // POST: wallet
    [HttpPost]
    public async Task<IActionResult> CreateWallet([FromBody] WalletDto wallet)
    {
        var result = await _walletService.CreateWalletAsync(wallet);
        return CreatedAtAction("GetWalletByDocument", new { id = result.DocumentId }, result);
    }

    // PUT: wallet/5
    [HttpPut("{id}")]
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

    // DELETE: wallet/5
    [HttpDelete("{id}")]
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
