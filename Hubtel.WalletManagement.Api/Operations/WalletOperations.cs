using Hubtel.WalletManagement.Api.Interfaces;
using Hubtel.WalletManagement.Api.Models;

namespace Hubtel.WalletManagement.Api.Operations
{
    public class WalletOperations(IWalletRepository walletRepository) : IWalletOperations
    {
        private readonly IWalletRepository _walletRepository = walletRepository;

        public async Task AddWalletAsync(Wallet wallet)
        {
            await _walletRepository.AddWalletAsync(wallet);
        }

        public async Task<Wallet> GetWalletAsync(Guid id)
        {
            return await _walletRepository.GetWalletAsync(id);
        }

        public async Task UpdateWalletAsync(Guid id, Wallet wallet)
        {
            await _walletRepository.UpdateWalletAsync(id, wallet);
        }

        public async Task DeleteWalletAsync(Guid id)
        {
            await _walletRepository.DeleteWalletAsync(id);
        }
    }

}
