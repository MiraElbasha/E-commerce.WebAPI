using E_Commerce.Domain.Contracts;
using E_Commerce.Infrastructure.Identity.Data;
using E_Commerce.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.Seeding
{
    public class IdentityDataSeeder : IDataSeeder
    {
        private readonly StoreIdentityDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<IdentityDataSeeder> _logger;

        public IdentityDataSeeder(StoreIdentityDbContext dbContext , UserManager<ApplicationUser> userManager , RoleManager<IdentityRole> roleManager , ILogger<IdentityDataSeeder> logger)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }
        public async Task SeedAsync(CancellationToken ct = default)
        {
            try
            {
                var PendingMigrations = await _dbContext.Database.GetPendingMigrationsAsync(ct);
                if (PendingMigrations.Any()) 
                {
                    await _dbContext.Database.MigrateAsync(ct);
                }
                if (! await _roleManager.Roles.AnyAsync(ct))
                {
                    await _roleManager.CreateAsync(new IdentityRole("Admin"));
                    await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
                }
                if (! await _userManager.Users.AnyAsync(ct))
                {
                    var Admin = new ApplicationUser
                    {
                        DisplayName = "Mira Elbasha",
                        Email = "Mira2malak@gmail.com",
                        UserName = "Mira",
                        PhoneNumber = "01255788843",

                    };
                    var CreateResult = await _userManager.CreateAsync(Admin, "P@ssw0rd");
                    if (CreateResult.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(Admin, "Admin");
                    }
                    else 
                    {
                        _logger.LogWarning("Could Not Seed Default Admin User : {Error}", string.Join(";" , CreateResult.Errors.Select(e => e.Description)));
                    }
                }
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Identity Data Seeding Failed");
            }
        }
    }
}
