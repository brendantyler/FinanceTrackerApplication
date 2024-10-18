using FinanceTrackerApplication.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;

namespace FinanceTrackerApplication.Models.ViewModels
{
    public class SettingsVM
    {
        public FinanceTrackerApplicationUser User { get; set; }

        [Required]
        [MaxLength(15)]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }

        public string? OldPassword { get; set; }
        public string? NewPassword { get; set; }

    }
}
