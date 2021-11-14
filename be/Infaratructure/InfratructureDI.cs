using Application.Common.Interfaces;
using Domain.Entities.UserManagements;
using Infaratructure.HttpClientHelper;
using Infaratructure.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace Infaratructure
{
    public static class InfratructureDI
    {
        public static IServiceCollection AddInfatructure(this IServiceCollection services, IConfiguration config)
        {
            if (config.GetSection("UseInMemory").Value == "true")
            {
                services.AddDbContext<ApplicationDbContext>(
                options =>
                    options.UseInMemoryDatabase("MemoryBaseDataBase")
                );
            }
            else
            {
                services.AddDbContext<ApplicationDbContext>(
                options =>
                    options.UseNpgsql(config.GetConnectionString("DefaultConnection"),
                        builder =>
                            builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName))
                );
            }
            

            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());

            services
                .AddDefaultIdentity<ApplicationUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            //Account
            services.Configure<IdentityOptions>(o =>
            {
                o.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
                o.Lockout.MaxFailedAccessAttempts = 6;
            });

            //Config rule identity user
            services.Configure<IdentityOptions>(options => options.Password.RequireDigit = false);

            services.AddTransient<IIdentityService, IdentityService>();

            //Add Authen
            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = config["JWT:ValidIssuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:Secret"]))
                    };
                });

            services.AddAuthorization(oprions =>
                oprions.AddPolicy("CanPurge", policy => policy.RequireRole("Administrator"))
                );

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<HttpClientAuthorizationDelegatingHandler>();
            services.AddSingleton<TimeoutDelegatingHandler>();
            services.AddSingleton<HttpClientRequestIdDelegatingHandler>();

            return services;
        }
    }
}
