using E_Commerce.Application.Contracts;
using E_Commerce.Application.Services;
using E_Commerce.Domain.Contracts;
using E_Commerce.Infrastructure.Data;
using E_Commerce.Infrastructure.Identity.Data;
using E_Commerce.Infrastructure.Identity.Entities;
using E_Commerce.Infrastructure.Identity.Services;
using E_Commerce.Infrastructure.Payments;
using E_Commerce.Infrastructure.Repositories;
using E_Commerce.Infrastructure.Seeding;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static E_Commerce.Infrastructure.Identity.Services.TokenService;

namespace E_Commerce.Infrastructure
{
    public static class InfrastructureServicesRegistrations
    {
        public static IServiceCollection AddInfraStuctureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<StoreDbContext>(
                options =>
                {
                    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
                });
            //key == "Catalog"
            //data seeding service
            services.AddKeyedScoped<IDataSeeder, CatalogDataSeeder>("Catalog");
            services.AddKeyedScoped<IDataSeeder, IdentityDataSeeder>("Identity");
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            #region Redis
            services.AddSingleton<IConnectionMultiplexer>(config =>
            {
                return ConnectionMultiplexer.Connect(configuration.GetConnectionString("RedisConnection")!);

            });
                services.AddScoped<IBasketRepository, BasketRepository>();
            #endregion

            #region Identity
            services.AddDbContext<StoreIdentityDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("IdentityConnection"));
            });
            services.AddIdentityCore<ApplicationUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<StoreIdentityDbContext>();

            services.AddScoped<IIdentityService, IdentityService>();

            services.AddScoped<ITokenService, TokenService>();

            #region Token
            services.Configure<JWTSettings>(configuration.GetSection("JWT"));

            var jwtSettings = configuration.GetSection("JWT").Get<JWTSettings>()
       ?? throw new InvalidOperationException("JWT Settings Is Not Configured");

            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme =
                    JwtBearerDefaults.AuthenticationScheme;

                opt.DefaultChallengeScheme =
                    JwtBearerDefaults.AuthenticationScheme;

            })
       .AddJwtBearer(opt =>
       {
           opt.SaveToken = true;
           opt.RequireHttpsMetadata = true;

           opt.TokenValidationParameters = new TokenValidationParameters
           {
               ValidateIssuer = true,
               ValidIssuer = jwtSettings.Issuer,

               ValidateAudience = true,
               ValidAudience = jwtSettings.Audience,

               ValidateLifetime = true,

               ValidateIssuerSigningKey = true,

               RequireExpirationTime = true,
               RequireSignedTokens = true,

               ClockSkew = TimeSpan.Zero,

               IssuerSigningKey = new SymmetricSecurityKey(
                   Encoding.UTF8.GetBytes(jwtSettings.SecretKey)
               )
           };
       });

            #endregion

            #endregion

            services.AddSingleton<IPaymentGatway, StripePaymentGatway>();
            //basket
            services.AddScoped<IBasketService, BasketService>();

            //cashing
            services.AddSingleton<ICasheRepository , CasheRepository>();
            services.AddSingleton<ICasheService, CasheService>();
            return services;
        }
    }
}

