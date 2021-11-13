using Domain.Entities.UserManagements;
using Infaratructure;
using Infaratructure.SeedData;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Presentation
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
           var host = CreateHostBuilder(args).Build();

            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            try
            {
                var context = services.GetRequiredService<ApplicationDbContext>();
                var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                await UserDefault.CreateSeedUserAsync(context, userManager, roleManager).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

                logger.LogError(ex, "Err Seed data");
                throw;
            }

            await host.RunAsync().ConfigureAwait(false);
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());
    }
}
