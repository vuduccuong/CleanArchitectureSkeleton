using Domain.Entities.UserManagements;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infaratructure.SeedData
{
    public static class UserDefault
    {
        private const string DEFAULT_PASSWORD = "G7e3KSMED!";
        public static async Task CreateSeedUserAsync(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var superAdminRole = new IdentityRole("SuperAdmin");

            if (roleManager?.Roles.All(r => r.Name != superAdminRole.Name) == true)
            {
                await roleManager.CreateAsync(superAdminRole).ConfigureAwait(false);
            }

            var superAdminUser = new ApplicationUser
            {
                UserName = "cuongvd7",
                Email = "cuongvd7@gmail.com",
                SecurityStamp = Guid.NewGuid().ToString()
            };

            if (userManager?.Users.All(u => u.UserName != superAdminUser.UserName) == true)
            {
                await userManager.CreateAsync(superAdminUser, DEFAULT_PASSWORD).ConfigureAwait(false);
                await userManager.AddToRolesAsync(superAdminUser, new[] { superAdminRole.Name }).ConfigureAwait(false);

                var adminProfile = new UserProfile
                {
                    Uid = Guid.NewGuid(),
                    UserId = superAdminUser.Id,
                    FullName = "Siêu cấp Vjp Pro",
                    ApplicationUser = superAdminUser,
                    NickName = "NickWater"
                };
                context.USR_UserProfiles.Add(adminProfile);

                await context.SaveChangesAsync().ConfigureAwait(false);
            }

            var leadRole = new IdentityRole("Leader");
            if (roleManager?.Roles.All(r => r.Name != leadRole.Name) == true)
            {
                await roleManager.CreateAsync(leadRole).ConfigureAwait(false);
            }

            var leadUser = new ApplicationUser
            {
                UserName = "lead1",
                Email = "lead@gmail.com",
                SecurityStamp = Guid.NewGuid().ToString()
            };

            if (userManager?.Users.All(u => u.UserName != leadUser.UserName) == true)
            {
                await userManager.CreateAsync(leadUser, DEFAULT_PASSWORD).ConfigureAwait(false);
                await userManager.AddToRolesAsync(leadUser, new[] { leadRole.Name }).ConfigureAwait(false);

                var leadProfile = new UserProfile
                {
                    Uid = Guid.NewGuid(),
                    UserId = leadUser.Id,
                    FullName = "Siêu cấp Lead Pro",
                    ApplicationUser = leadUser,
                    NickName = "LeadWater"
                };
                context.USR_UserProfiles.Add(leadProfile);

                await context.SaveChangesAsync().ConfigureAwait(false);
            }
        }
    }
}
