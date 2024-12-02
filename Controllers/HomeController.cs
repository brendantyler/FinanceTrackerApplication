using FinanceTrackerApplication.Areas.Identity.Data;
using FinanceTrackerApplication.Data;
using FinanceTrackerApplication.Models;
using FinanceTrackerApplication.Models.ViewModels;
using FinanceTrackerApplication.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using NuGet.Protocol;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Security.Claims;


namespace FinanceTrackerApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserService _userService;
        private readonly ILogger<HomeController> _logger;
        /// <summary>
        /// Application DB context
        /// </summary>
        protected FinanceTrackerApplicationContext _applicationDbContext { get; set; }

        /// <summary>
        /// User manager - attached to application DB context
        /// </summary>
        protected UserManager<FinanceTrackerApplicationUser> _userManager { get; set; }

        public HomeController(ILogger<HomeController> logger, IUserService userService, FinanceTrackerApplicationContext applicationContext, UserManager<FinanceTrackerApplicationUser> userManager)
        {
            _applicationDbContext = applicationContext;
            _userManager = userManager;
            _logger = logger;
            _userService = userService;
        }

        public async Task<IActionResult> Index(string? message)
        {
            FinanceTrackerApplicationUser? user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return View();
            }
            else
            {
                if (user.newUser)
                {
                    return RedirectToAction("Welcome", "Home", user);
                }

                return RedirectToAction("Dashboard", "Home", user);
            }
        }

        // User Authorized Pages
        [Authorize]
        public IActionResult Welcome(FinanceTrackerApplicationUser user, string? message )
        {
            return View(user);
        }

        [Authorize]
        public IActionResult Dashboard(FinanceTrackerApplicationUser user, string? message)
        {
            ViewData["Message"] = message;
            ViewData["TotalBalance"] = _userService.GetTotalBalance(user);

            List<Account> Accounts = _applicationDbContext.Account.Where(x => x.User == user).ToList();
            user.Accounts = Accounts;

            return View(user);
        }

        [Authorize]
        public async Task<IActionResult> Settings(string? message)
        {
            SettingsVM vm = new SettingsVM();
            var user = await _userManager.GetUserAsync(User);

            if (user != null)
            {
                vm.User = user;
                vm.Username = user.UserName;
                vm.Email = user.Email;
            }
            else
            {
                return RedirectToAction("Index");
            }

            ViewData["Message"] = message;

            return View(vm);
        }

        [Authorize]
        //TODO: Add Input to page
        public async Task<IActionResult> ChangeInformation(SettingsVM vm)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Settings");
            }

            if(vm.OldPassword != null && vm.NewPassword != null)
            {
                var result = await _userManager.ChangePasswordAsync(user, vm.OldPassword, vm.NewPassword);
                await _userManager.UpdateAsync(user);


                if (result.Succeeded)
                {
                    return RedirectToAction("Settings", new { message = "Password Changed" });
                }
                else
                {
                    return RedirectToAction("Settings", new { message = "Error: Password not changed" });
                }
            }
            else if (vm.Username != null)
            {
                user.UserName = vm.Username;

                await _userManager.UpdateAsync(user);

                return RedirectToAction("Settings", new { message = "Username Changed" });

            }
            else if (vm.Email != null)
            {
                user.Email = vm.Email;
                await _userManager.UpdateAsync(user);

                return RedirectToAction("Settings", new { message = "Email Changed" });

            }
            
            return RedirectToAction("Settings");
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> SetCurrency([Bind("CurrencyCode")] FinanceTrackerApplicationUser user)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            if (currentUser != null)
            {
                currentUser.CurrencyCode = user.CurrencyCode;
                currentUser.newUser = false;

                await _userManager.UpdateAsync(currentUser);
                await _applicationDbContext.SaveChangesAsync();
            }
            else
            {
                return RedirectToAction("Welcome", new { message = "Error: User not found" });
            }

            return RedirectToAction("Index", new { message = "Welcome to the App!" });
        }

        // Non protected pages
        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Currencies()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
