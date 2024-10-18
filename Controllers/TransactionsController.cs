
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FinanceTrackerApplication.Data;
using FinanceTrackerApplication.Models;
using FinanceTrackerApplication.Models.ViewModels;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Http.Headers;
using FinanceTrackerApplication.ServiceInterfaces;
using FinanceTrackerApplication.Services;
using FinanceTrackerApplication.Models.ErrorModels;
using FinanceTrackerApplication.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace FinanceTrackerApplication.Controllers
{
    [Authorize]
    public class TransactionsController : Controller
    {
        private readonly FinanceTrackerApplicationContext _context;
        private readonly UserManager<FinanceTrackerApplicationUser> _userManager;
        private ITransferService _transferService;

        public TransactionsController(FinanceTrackerApplicationContext context, ITransferService transferService, UserManager<FinanceTrackerApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            _transferService = transferService;
        }

        // GET: Transactions
        public async Task<IActionResult> Index()
        {
            List<Transaction> Transactions = new List<Transaction>();
            FinanceTrackerApplicationUser user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                Transactions = await _context.Transaction.ToListAsync();
            }
            else
            {
                var UserAccounts = await _context.Account.Where(x => x.User == user).ToListAsync();

                foreach (Account account in UserAccounts)
                {
                    List<Transaction> accountTransactions = await _context.Transaction.Where(x => x.AccountIntoId == account.Id || x.AccountOutOfId == account.Id).ToListAsync();

                    Transactions.AddRange(accountTransactions);
                }

                Transactions = Transactions.OrderByDescending(x => x.CreatedAt).ToList();
            }

            return View(Transactions);
        }

        // GET: Transactions/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transaction
                .FirstOrDefaultAsync(m => m.Id == id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        // GET: Transactions/Create
    
        public async Task<IActionResult> Create()
        {
            var Accounts = await _context.Account.ToListAsync();
            var user = await _userManager.GetUserAsync(User);

            ViewData["AccountInto"] = new SelectList(Accounts, "Id", "Name");
            ViewData["AccountOutOf"] = new SelectList(Accounts, "Id", "Name");
            
            TransactionVM transactionVM = new TransactionVM()
            {
                CurrentUser = user,
                Transaction = new Transaction(),
            };

            return View(transactionVM);
        }

        // POST: Transactions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TransactionVM transactionVM)
        {
            if (ModelState.IsValid)
            {
                TransferStatusModel transfered = await _transferService.TransferMoneyAsync(transactionVM.Transaction.IsInternalTransfer, transactionVM.IsDeposit, transactionVM.AccountOutOf, transactionVM.AccountInto, transactionVM.Transaction.Amount);

                if (!transfered.Success)
                {
                    return RedirectToAction("Index", "Home", new { message = transfered.Message });
                }

                var AccountInto = await _context.Account.FirstOrDefaultAsync(x => x.Id == transactionVM.AccountInto);
                var AccountOut = await _context.Account.FirstOrDefaultAsync(x => x.Id == transactionVM.AccountOutOf);

                Transaction transaction = new Transaction
                {
                    AccountIntoId = transactionVM.AccountInto,
                    Amount = transactionVM.Transaction.Amount,
                    IsInternalTransfer = transactionVM.Transaction.IsInternalTransfer,
                    ExternalAccountName = transactionVM.Transaction.ExternalAccountName,
                    Repeating = transactionVM.Transaction.Repeating,
                    Frequency = transactionVM.Transaction.Frequency,
                    CreatedAt = DateTime.Now
                };

                if(transactionVM.Transaction.IsInternalTransfer )
                {
                    if(AccountOut == null || AccountInto == null)
                    {
                        return RedirectToAction("Index", "Home", new { message = "Error: One or both of the accounts you are trying to transfer money to/from could not be found" });
                    }
                    transaction.AccountOutOfId = AccountOut.Id;
                    transaction.Description = $"Internal Transfer of {transactionVM.Transaction.Amount} From {AccountOut.Name} to {AccountInto.Name}";
                }
                else
                {
                    if (transactionVM.IsDeposit)
                    {
                        if (AccountInto == null)
                        {
                            return RedirectToAction("Index", "Home", new { message = "Error: The account you are trying to transfer money into does not exist" });
                        }

                        transaction.AccountOutOfId = null;

                        if (transaction.Description == null)
                        { 
                            transaction.Description = $"Deposit of {transactionVM.Transaction.Amount} into {AccountInto.Name}";
                        }
                    }
                    else
                    {
                        transaction.AccountOutOfId = transactionVM.AccountOutOf;
                        transaction.Description = $"Withdrawal of {transactionVM.Transaction.Amount} from {AccountInto}";

                        return RedirectToAction("Index", "Home", new { message = transfered.Message });
                    }
                }

                _context.Add(transaction);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        // GET: Transactions/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transaction.FindAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }
            return View(transaction);
        }

        // POST: Transactions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,IsInternalTransfer,AccountOutOfId,ExternalAccountName,AccountIntoId,Amount,Description,Created,Repeating,Frequency")] Transaction transaction)
        {
            if (id != transaction.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(transaction);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransactionExists(transaction.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(transaction);
        }

        // GET: Transactions/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transaction
                .FirstOrDefaultAsync(m => m.Id == id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        // POST: Transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var transaction = await _context.Transaction.FindAsync(id);
            if (transaction != null)
            {
                _context.Transaction.Remove(transaction);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TransactionExists(Guid id)
        {
            return _context.Transaction.Any(e => e.Id == id);
        }
    }
}
