using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blp.NetCoreLearning.WebApp
{
    public class IdentityRoles
    {
        public const string Administrator = "Administrator";
        public const string Tester = "Tester";

        public static async Task InitializeRoles(IServiceProvider serviceProvider, IConfiguration Configuration)
        {
            RoleManager<IdentityRole> roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            UserManager<IdentityUser> userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            string[] roleNames = { Administrator, Tester};
            IdentityResult roleResult;

            foreach (string roleName in roleNames)
            {
                bool roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            IdentityUser user = await userManager.FindByEmailAsync("BPERRY@BUDDVANLINES.COM");
            bool isInRole = await userManager.IsInRoleAsync(user, Administrator);

            var logger = serviceProvider.GetRequiredService<ILogger<IdentityRoles>>();

            if (!isInRole)
            {
                await userManager.AddToRoleAsync(user, Administrator);
                logger.Log(LogLevel.Information, "User bperry@buddvanlines.com was added to role.");
            }
            else
            {
                logger.Log(LogLevel.Information, "User bperry@buddvanlines.com is already in role.");
            }
        }
    }
}
