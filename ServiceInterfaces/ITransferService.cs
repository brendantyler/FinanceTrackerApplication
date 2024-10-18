
using FinanceTrackerApplication.Models.ErrorModels;

namespace FinanceTrackerApplication.ServiceInterfaces
{
    //This interface handles all transfers of money

    public interface ITransferService
    {
        //This method transfers money from one account to another
        Task<TransferStatusModel> TransferMoneyAsync(bool internalTransfer, bool isDeposit, Guid? accountOutId, Guid? accountInId, decimal amount);
    }
}
