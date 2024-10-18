using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using FinanceTrackerApplication.Models;
using FinanceTrackerApplication.Data;
using FinanceTrackerApplication.Areas.Identity.Data;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FinanceTrackerApplication.Data;
public class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using (var context = new FinanceTrackerApplicationContext(
            serviceProvider.GetRequiredService<DbContextOptions<FinanceTrackerApplicationContext>>()))
        {
            // Look for any users.
            if (context.Users.Any())
            {
                return;   // DB has been seeded
            }

            context.Users.RemoveRange(context.Users);

            Guid userId = Guid.NewGuid();
            Guid budgetId = Guid.NewGuid();
            Guid categoryId = Guid.NewGuid();
            Guid accountId = Guid.NewGuid();

            context.Users.AddRange(
                new FinanceTrackerApplicationUser 
                {
                    Id = userId.ToString(),
                    UserName = "adminTest",
                    Email = "Test@test.com",
                });

            context.SaveChanges();
        };
    }
}

