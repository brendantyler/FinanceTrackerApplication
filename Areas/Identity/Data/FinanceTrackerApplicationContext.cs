﻿using FinanceTrackerApplication.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using FinanceTrackerApplication.Models;

namespace FinanceTrackerApplication.Data;

public class FinanceTrackerApplicationContext : IdentityDbContext<FinanceTrackerApplicationUser>
{
    public FinanceTrackerApplicationContext(DbContextOptions<FinanceTrackerApplicationContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }

public DbSet<FinanceTrackerApplication.Models.Account> Account { get; set; } = default!;

public DbSet<FinanceTrackerApplication.Models.Transaction> Transaction { get; set; } = default!;
}
