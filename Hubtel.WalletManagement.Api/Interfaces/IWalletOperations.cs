using Hubtel.WalletManagement.Api.Models;

namespace Hubtel.WalletManagement.Api.Interfaces
{
    public interface IWalletOperations
    {
        Task AddWalletAsync(Wallet wallet);
        Task<Wallet> GetWalletAsync(Guid id);
        Task UpdateWalletAsync(Guid id, Wallet wallet);
        Task DeleteWalletAsync(Guid id);
    }
}
