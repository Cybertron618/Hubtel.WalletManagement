using System;
using System.Threading.Tasks;
using Hubtel.WalletManagement.Api.Interfaces;
using Hubtel.WalletManagement.Api.Models;

namespace Hubtel.WalletManagement.Api.Services
{
    public class WalletService(IWalletOperations walletOperations) : IWalletService
    {
        private readonly IWalletOperations _walletOperations = walletOperations;

        public async Task AddWalletAsync(Wallet wallet)
        {
            await _walletOperations.AddWalletAsync(wallet);
        }

        public async Task<Wallet> GetWalletAsync(Guid id)
        {
            return await _walletOperations.GetWalletAsync(id);
        }

        public async Task UpdateWalletAsync(Guid id, Wallet wallet)
        {
            await _walletOperations.UpdateWalletAsync(id, wallet);
        }

        public async Task DeleteWalletAsync(Guid id)
        {
            await _walletOperations.DeleteWalletAsync(id);
        }
    }
}
