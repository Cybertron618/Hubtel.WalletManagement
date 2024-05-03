namespace Hubtel.WalletManagement.Api.Models
{
    public class Wallet
    {
        public Guid Id { get; set; }
        public string? AccountName { get; set; }
        public string? AccountNumber { get; set; }
        public decimal? Balance { get; set; }
    }
}
