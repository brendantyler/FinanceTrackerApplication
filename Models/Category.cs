using System.ComponentModel.DataAnnotations;

namespace FinanceTrackerApplication.Models
{
    public class Category
    {
        [Key]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }

        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }
    }
}
