using FinanceTrackerApplication.Areas.Identity.Data;
using FinanceTrackerApplication.Data;
using FinanceTrackerApplication.Models;
using FinanceTrackerApplication.Models.ErrorModels;
using FinanceTrackerApplication.ServiceInterfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace FinanceTrackerApplication.Services
{
    public class TransferService : ITransferService
    {
        private FinanceTrackerApplicationContext _context;
        private readonly UserManager<FinanceTrackerApplicationUser> _userManager;
        public TransferService(FinanceTrackerApplicationContext context, UserManager<FinanceTrackerApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<TransferStatusModel> ValidateAccountsAsync(bool internalTransfer, bool isDeposit, Guid? accountOutId, Guid? accountInId, decimal amount)
        {
            TransferStatusModel status = new TransferStatusModel();
            try
            {
                var accountOut = await _context.Account.FirstOrDefaultAsync(x => x.Id == accountOutId);
                var accountInto = await _context.Account.FirstOrDefaultAsync(x => x.Id == accountInId);

                if (internalTransfer)
                {
                    if (accountOut == null || accountInto == null)
                    {
                        status.Success = false;
                        status.Message = "Error: One or both of the accounts you are trying to transfer money to/from could not be found";
                        return status;
                    }
                }
                else
                {
                    if (isDeposit)
                    {
                        if (accountInto == null)
                        {
                            status.Success = false;
                            status.Message = "Error: The account you are trying to transfer money to does not exist";
                            return status;
                        }
                    }
                    else
                    {                      
                        if (accountOut == null)
                        {
                            status.Success = false;
                            status.Message = "Error: The account you are trying to transfer money from does not exist";
                            return status;
                        }
                    }
                }

                status.Success = true;
                status.AccountOut = accountOut;
                status.AccountIn = accountInto;
            }
            catch (Exception ex) 
            { 
                status.Success = false;
                status.Message = ex.ToString();
            }

            return status;
        }

        public async Task<TransferStatusModel> TransferMoneyAsync(bool internalTransfer, bool isDeposit, Guid? accountOutId, Guid? accountInId, decimal amount)
        {
            TransferStatusModel validated = await ValidateAccountsAsync(internalTransfer, isDeposit, accountOutId, accountInId, amount);

            try
            {
                if (!validated.Success)
                {
                    return validated;
                }

                var accountOut = validated.AccountOut;
                var accountIn = validated.AccountIn;

                if (internalTransfer && accountOut != null && accountIn != null) 
                {
                    accountOut.Balance -= amount;
                    accountIn.Balance += amount;

                    _context.Update(accountOut);
                    _context.Update(accountIn);
                }
                else
                {
                    if (isDeposit && accountIn != null)
                    {
                        accountIn.Balance += amount;
                        _context.Update(accountIn);
                    }
                    else if (!isDeposit && accountOut != null)
                    {
                        accountOut.Balance -= amount;
                        _context.Update(accountOut);
                    }
                    else 
                    { 
                        validated.Success = false;
                        validated.Message = "There was in issue when dealing with an external transfer";
                    }
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString() + "There was an issue when transferring money");
            }

            return validated;
        }
    }
}
