using Microsoft.EntityFrameworkCore;
using Hubtel.WalletManagement.Api.Models;

namespace Hubtel.WalletManagement.Api.Data
{
    public class WalletDbContext(DbContextOptions<WalletDbContext> options) : DbContext(options)
    {
        public DbSet<Wallet> Wallets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure the entity mappings, relationships, etc.
            modelBuilder.Entity<Wallet>().ToTable("Wallets");
            modelBuilder.Entity<Wallet>().HasKey(w => w.Id);
            modelBuilder.Entity<Wallet>().Property(w => w.AccountName).IsRequired();
            modelBuilder.Entity<Wallet>().Property(w => w.AccountNumber).IsRequired();
            modelBuilder.Entity<Wallet>().Property(w => w.Balance).HasColumnType("decimal(18, 2)");
        }
    }
}
