using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Hubtel.WalletManagement.Api.Models;
using Hubtel.WalletManagement.Api.Interfaces;

namespace Hubtel.WalletManagement.Api.Controllers
{
    [ApiController]
    [Route("api/wallets")]
    public class WalletController(IWalletService walletService) : ControllerBase
    {
        private readonly IWalletService _walletService = walletService ?? throw new ArgumentNullException(nameof(walletService));

        [HttpPost]
        public async Task<IActionResult> AddWallet([FromBody] Wallet wallet)
        {
            await _walletService.AddWalletAsync(wallet);
            return CreatedAtAction(nameof(GetWallet), new { id = wallet.Id }, wallet);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetWallet(Guid id)
        {
            var wallet = await _walletService.GetWalletAsync(id);
            if (wallet == null)
            {
                return NotFound();
            }
            return Ok(wallet);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWallet(Guid id, [FromBody] Wallet wallet)
        {
            await _walletService.UpdateWalletAsync(id, wallet);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWallet(Guid id)
        {
            await _walletService.DeleteWalletAsync(id);
            return NoContent();
        }
    }
    
}
