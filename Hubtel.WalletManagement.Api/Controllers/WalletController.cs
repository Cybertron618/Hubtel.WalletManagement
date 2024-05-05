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
            try
            {
                await _walletService.AddWalletAsync(wallet);
                return CreatedAtAction(nameof(GetWallet), new { id = wallet.Id }, wallet);
            }
            catch
            {
                // Log the exception or handle it accordingly
                return StatusCode(500, "An error occurred while adding the wallet.");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetWallet(Guid id)
        {
            try
            {
                var wallet = await _walletService.GetWalletAsync(id);
                if (wallet == null)
                {
                    return NotFound();
                }
                return Ok(wallet);
            }
            catch
            {
                // Log the exception or handle it accordingly
                return StatusCode(500, "An error occurred while fetching the wallet.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWallet(Guid id, [FromBody] Wallet wallet)
        {
            try
            {
                await _walletService.UpdateWalletAsync(id, wallet);
                return Ok();
            }
            catch
            {
                // Log the exception or handle it accordingly
                return StatusCode(500, "An error occurred while updating the wallet.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWallet(Guid id)
        {
            try
            {
                await _walletService.DeleteWalletAsync(id);
                return NoContent();
            }
            catch
            {
                // Log the exception or handle it accordingly
                return StatusCode(500, "An error occurred while deleting the wallet.");
            }
        }
    }
}
