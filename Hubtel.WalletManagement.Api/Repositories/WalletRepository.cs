using System;
using System.Threading.Tasks;
using Hubtel.WalletManagement.Api.Models;
using StackExchange.Redis;
using Hubtel.WalletManagement.Api.Interfaces;

namespace Hubtel.WalletManagement.Api.Repository
{
    public class WalletRepository(ConnectionMultiplexer redis) : IWalletRepository
    {
        private readonly ConnectionMultiplexer _redis = redis ?? throw new ArgumentNullException(nameof(redis));

        public async Task AddWalletAsync(Wallet wallet)
        {
            var database = _redis.GetDatabase();
            await database.HashSetAsync($"wallet:{wallet.Id}",
            [
                new HashEntry("AccountName", wallet.AccountName),
                new HashEntry("AccountNumber", wallet.AccountNumber),
                new HashEntry("Balance", wallet.Balance.ToString())
            ]);
        }

        public async Task<Wallet> GetWalletAsync(Guid id)
        {
            var database = _redis.GetDatabase();
            var values = await database.HashGetAllAsync($"wallet:{id}");

            if (values.Length == 0)
            {
                throw new KeyNotFoundException($"Wallet with ID {id} not found.");
            }

            // Extract values and handle null cases
            var accountName = values[0].Value.IsNull ? null : values[0].Value.ToString();
            var accountNumber = values[1].Value.IsNull ? null : values[1].Value.ToString();
            var balanceString = values[2].Value.IsNull ? null : values[2].Value.ToString();

            // Check for null values and handle them
            if (accountName == null || accountNumber == null || balanceString == null)
            {
                throw new InvalidOperationException($"Invalid data retrieved for wallet with ID {id}");
            }

            if (!decimal.TryParse(balanceString, out decimal balance))
            {
                throw new InvalidOperationException($"Failed to parse balance for wallet with ID {id}");
            }

            return new Wallet
            {
                Id = id,
                AccountName = accountName,
                AccountNumber = accountNumber,
                Balance = balance
            };
        }

        public async Task UpdateWalletAsync(Guid id, Wallet wallet)
        {
            var database = _redis.GetDatabase();
            await database.HashSetAsync($"wallet:{id}",
            [
                new HashEntry("AccountName", wallet.AccountName),
                new HashEntry("AccountNumber", wallet.AccountNumber),
                new HashEntry("Balance", wallet.Balance.ToString())
            ]);
        }

        public async Task DeleteWalletAsync(Guid id)
        {
            var database = _redis.GetDatabase();
            await database.KeyDeleteAsync($"wallet:{id}");
        }

        public async Task<bool> WalletExistsAsync(Guid id)
        {
            var database = _redis.GetDatabase();
            return await database.KeyExistsAsync($"wallet:{id}");
        }
    }
}
