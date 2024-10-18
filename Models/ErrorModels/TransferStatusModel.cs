using Microsoft.Identity.Client;

namespace FinanceTrackerApplication.Models.ErrorModels
{
    public class TransferStatusModel
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public Account? AccountIn { get; set; }
        public Account? AccountOut { get; set; }
    }
}
