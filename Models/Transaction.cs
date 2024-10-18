using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Permissions;

namespace FinanceTrackerApplication.Models
{
    public class Transaction
    {
        [Key]
        public Guid Id { get; set; }
        public bool IsInternalTransfer { get; set; }
        public Guid? AccountOutOfId { get; set; }
        public string? ExternalAccountName { get; set; }
        public Guid? AccountIntoId { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        public bool Repeating { get; set; }
        public Freq Frequency { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public Transaction()
        {
            Id = Guid.NewGuid();
            IsInternalTransfer = false;
            Repeating = false;
            CreatedAt = DateTime.Now;
        }
    }
}