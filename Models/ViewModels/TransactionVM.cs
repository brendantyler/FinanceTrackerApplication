using FinanceTrackerApplication.Areas.Identity.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace FinanceTrackerApplication.Models.ViewModels
{
    public class TransactionVM
    {
        public FinanceTrackerApplicationUser? CurrentUser { get; set; }
        public Transaction Transaction { get; set; }
        public bool IsDeposit { get; set; }
        public Guid? AccountOutOf { get; set; }
        public Guid? AccountInto { get; set; }
    }

}
