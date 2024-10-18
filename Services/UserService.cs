using FinanceTrackerApplication.Areas.Identity.Data;
using FinanceTrackerApplication.Data;
using FinanceTrackerApplication.Models;
using FinanceTrackerApplication.ServiceInterfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace FinanceTrackerApplication.Services
{
    public class UserService : IUserService
    {
        private UserManager<FinanceTrackerApplicationUser> _userManager;
        private FinanceTrackerApplicationContext _context;
        public UserService(UserManager<FinanceTrackerApplicationUser> userManager, FinanceTrackerApplicationContext applicationContext) 
        {
            _userManager = userManager;
            _context = applicationContext;
        }

        //User Requests
        public async Task<FinanceTrackerApplicationUser> GetUserById(Guid UserId)
        {
            try
            {
                FinanceTrackerApplicationUser user = await _context.Users.FirstAsync(x => x.Id == UserId.ToString());
                user.Accounts = _context.Account.Where(x => x.User == user).ToList();

                return user ?? throw new Exception("Error: We were unable to get your data from our records");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // User CRUD
        public async Task UpdateUser(FinanceTrackerApplicationUser user)
        {
            try
            {
                await _userManager.UpdateAsync(user);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task DeleteUser(FinanceTrackerApplicationUser user)
        {
            try
            {
                await _userManager.DeleteAsync(user);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public decimal GetTotalBalance(FinanceTrackerApplicationUser user)
        {
            try
            {
                List<Account> Accounts = _context.Account.Where(x => x.User == user).ToList();
                decimal totalBalance = 0;

                foreach (Account account in Accounts)
                {
                    if (account.AccountType == AccountType.Credit || account.AccountType == AccountType.Loan)
                    {
                        totalBalance -= account.Balance;
                    }
                    else
                    {
                        totalBalance += account.Balance;
                    }
                }

                totalBalance = decimal.Round(totalBalance, 2);

                return totalBalance;
            }
            catch
            {
                return 0.00M;
            }
        }
    }
}
