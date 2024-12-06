using FinanceTrackerApplication.Areas.Identity.Data;
using FinanceTrackerApplication.Models;

namespace FinanceTrackerApplication.ServiceInterfaces
{
    public interface IUserService
    {
        //User Requests
        Task<FinanceTrackerApplicationUser> GetUserById(Guid UserId);

        // User CRUD
        Task UpdateUser(FinanceTrackerApplicationUser user);
        Task DeleteUser(FinanceTrackerApplicationUser user);
        decimal GetTotalBalance(FinanceTrackerApplicationUser user);
        List<Transaction> GetRecentTransactions(FinanceTrackerApplicationUser user);
    }
}
