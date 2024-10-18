using FinanceTrackerApplication.Areas.Identity.Data;

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
    }
}
