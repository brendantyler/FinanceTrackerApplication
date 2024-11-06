using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FinanceTrackerApplication.Data;
using FinanceTrackerApplication.Models;
using FinanceTrackerApplication.Migrations;
using FinanceTrackerApplication.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.AspNetCore.Authorization;
using NuGet.Packaging;

namespace FinanceTrackerApplication.Controllers
{
    [Authorize]
    public class AccountsController : Controller
    {
        private readonly FinanceTrackerApplicationContext _context;
        private readonly UserManager<FinanceTrackerApplicationUser> _userManager;

        public AccountsController(FinanceTrackerApplicationContext context, UserManager<FinanceTrackerApplicationUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        // GET: Accounts
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user != null)
            {
                List<Account> accounts = _context.Account.Where(x => x.User == user).ToList();

                return View(accounts);
            }

            return RedirectToAction("Index", "Home");
        }

        // GET: Accounts/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Account
                .FirstOrDefaultAsync(m => m.Id == id);

            if (account == null)
            {
                return NotFound();
            }
            account.User = await _userManager.GetUserAsync(User);
            account.Transactions = await _context.Transaction.Where(x => x.AccountIntoId == account.Id || x.AccountOutOfId == account.Id).OrderByDescending(x => x.CreatedAt).ToListAsync();

            return View(account);
        }

        // GET: Accounts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Accounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,AccountType,Balance")] Account account)
        {
            if (ModelState.IsValid)
            {
                account.User = await _userManager.GetUserAsync(User);
                switch (account.AccountType)
                {
                    case AccountType.Checking:
                        account.IsAsset = true;
                        break;
                    case AccountType.Savings:
                        account.IsAsset = true;
                        break;
                    case AccountType.Cash:
                        account.IsAsset = true;
                        break;
                    case AccountType.Investment:
                        account.IsAsset = true;
                        break;
                    case AccountType.Credit:
                        account.IsAsset = false;
                        break;
                    case AccountType.Loan:
                        account.IsAsset = false;
                        break;
                }

                _context.Add(account);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(account);
        }

        // GET: Accounts/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Account.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }
            return View(account);
        }

        // POST: Accounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,UserId,Name,AccountType,Balance")] Account account)
        {
            if (id != account.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(account);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccountExists(account.Id))
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
            return View(account);
        }

        // GET: Accounts/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Account
                .FirstOrDefaultAsync(m => m.Id == id);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        // POST: Accounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var account = await _context.Account.FindAsync(id);
            if (account != null)
            {
                _context.Account.Remove(account);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AccountExists(Guid id)
        {
            return _context.Account.Any(e => e.Id == id);
        }
    }
}
