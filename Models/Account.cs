using FinanceTrackerApplication.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceTrackerApplication.Models
{
    public class Account
    {
        [Key]
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public bool IsAsset { get; set; }
        public AccountType AccountType { get; set; }

        [Required]
        [Range(0.00, double.MaxValue, ErrorMessage = "Value Cannot be negative")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Balance { get; set; }
        public ICollection<Transaction>? Transactions { get; set; }
        public virtual FinanceTrackerApplicationUser? User { get; set; }
        public Account()
        {
            Id = Guid.NewGuid();
            Balance = 0.00m;
            Transactions = new List<Transaction>();
        }
    }

    public enum AccountType
    {
        Checking,
        Savings,
        Cash,
        Investment,
        Credit,
        Loan,
    }
}
