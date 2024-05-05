using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hubtel.WalletManagement.Api.Data;
using Hubtel.WalletManagement.Api.Interfaces;
using Hubtel.WalletManagement.Api.Models;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace Hubtel.WalletManagement.Api.Repositories
{
    public class WalletRepository(ConnectionMultiplexer redisConnection, WalletDbContext dbContext) : IWalletRepository
    {
        private readonly ConnectionMultiplexer _redisConnection = redisConnection ?? throw new ArgumentNullException(nameof(redisConnection));
        private readonly WalletDbContext _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

        public async Task AddWalletAsync(Wallet wallet)
        {
            var redisDatabase = _redisConnection.GetDatabase();
            await redisDatabase.HashSetAsync($"wallet:{wallet.Id}",
                [
                    new HashEntry("AccountName", wallet.AccountName),
                    new HashEntry("AccountNumber", wallet.AccountNumber),
                    new HashEntry("Balance", wallet.Balance.ToString())
                ]);

            _dbContext.Wallets.Add(wallet);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Wallet> GetWalletAsync(Guid id)
        {
            var redisDatabase = _redisConnection.GetDatabase();
            var redisValues = await redisDatabase.HashGetAllAsync($"wallet:{id}");

            if (redisValues.Length > 0)
            {
                var wallet = new Wallet
                {
                    Id = id,
                    AccountName = redisValues[0].Value,
                    AccountNumber = redisValues[1].Value,
                };

                // Check if the balance value is not null before parsing
                if (decimal.TryParse(redisValues[2].Value, out decimal balance))
                {
                    wallet.Balance = balance;
                }
                else
                {
                    // Handle the case where parsing fails (e.g., set a default value)
                    wallet.Balance = 0; // Or any other default value you prefer
                }

                return wallet;
            }

            var foundWallet = await _dbContext.Wallets.FindAsync(id);
            if (foundWallet != null)
            {
                return foundWallet;
            }

            // Handle the case where the wallet is not found
            throw new KeyNotFoundException($"Wallet with ID {id} not found.");
        }



        public async Task UpdateWalletAsync(Guid id, Wallet wallet)
        {
            var redisDatabase = _redisConnection.GetDatabase();
            await redisDatabase.HashSetAsync($"wallet:{id}",
                [
                    new HashEntry("AccountName", wallet.AccountName),
                    new HashEntry("AccountNumber", wallet.AccountNumber),
                    new HashEntry("Balance", wallet.Balance.ToString())
                ]);

            _dbContext.Entry(wallet).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteWalletAsync(Guid id)
        {
            var redisDatabase = _redisConnection.GetDatabase();
            await redisDatabase.KeyDeleteAsync($"wallet:{id}");

            var wallet = await _dbContext.Wallets.FindAsync(id);
            if (wallet != null)
            {
                _dbContext.Wallets.Remove(wallet);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<bool> WalletExistsAsync(Guid id)
        {
            var redisDatabase = _redisConnection.GetDatabase();
            var redisKeyExists = await redisDatabase.KeyExistsAsync($"wallet:{id}");

            if (!redisKeyExists)
            {
                return await _dbContext.Wallets.AnyAsync(w => w.Id == id);
            }

            return true;
        }
    }
}
