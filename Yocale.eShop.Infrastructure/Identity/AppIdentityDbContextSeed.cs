using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Yocale.eShop.Infrastructure.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedAsync(AppIdentityDbContext appIdentityDbContext, UserManager<ApplicationUser> userManager)
        {
            appIdentityDbContext.Database.Migrate();

            var defaultUser = new ApplicationUser { UserName = "demouser@yocale.com", Email = "demouser@yocale.com" };

            await userManager.CreateAsync(defaultUser, "Pass@word1");
        }
    }
}
