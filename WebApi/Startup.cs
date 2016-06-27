using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Entities.Contexts;
using Entities.Settings;
using Infrastructure.Encryption;
using Infrastructure.Password;
using FluentValidation;
using FluentValidation.Mvc6;
using WebApi.Domain.Factories;
using WebApi.Domain.Extensions;
using Entities.Models.Identity;
using IdentityServer4.Validation;
using WebApi.Services.Implementions;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Http;
using IdentityServer4.Services;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Diagnostics;
using Newtonsoft.Json;
using WebApi.Domain.Authentication;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.IO;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace WebApi
{
    public class Startup
    {
        private const string TokenAudience = "FrameworksAudience";
        private const string TokenIssuer = "FrameworksIssuer";
        private RsaSecurityKey key;
        private AuthTokenOptions tokenOptions;

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsEnvironment("Development"))
            {
                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);

                builder.AddUserSecrets();
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();

        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddApplicationInsightsTelemetry(Configuration);
            
            services.AddTransient(provider => new DatabaseConnections()
            {
                Database = Configuration.GetValue<string>("Connections:Database")
            })
            .AddTransient<IEncryptor, Encryptor>()
            .AddTransient(typeof(IStorageContext<>), typeof(StorageContext<>))
            .AddTransient<IStorageContext<AppUser>, StorageContext<AppUser>>()
            .AddTransient<UserStorageContext>()
            .AddTransient<IPasswordComplexity>(provider =>
                new PasswordComplexity()
                    .MinimumLength(8)
                    .LowerCase()
                    .UpperCase()
                    .Numbers()
                    .SpecialCharacters()
            )
            .AddTransient<IValidator<Features.Accounts.Models.RegisterUserModel>, Features.Accounts.Validations.RegisterUserModelValidation>();

            key = new RsaSecurityKey(GetRandomKey());

            services.AddSingleton(typeof(AuthTokenOptions), tokenOptions = new AuthTokenOptions()
            {
                Audience = TokenAudience,
                Issuer = TokenIssuer,
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.RsaSha256Signature)
            });

            services.AddMvc(options =>
            {
                options.Filters.Add(new AuthorizeFilter(new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser()
                    .Build()));
            }).AddFluentValidation(provider =>
            {
                provider.ValidatorFactory = new ValidatorFactory(services.BuildServiceProvider());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseApplicationInsightsRequestTelemetry();

            app.UseApplicationInsightsExceptionTelemetry();

            app.UseExceptionHandler(builder =>
            {
                builder.Use(async (context, next) =>
                {
                    var error = context.Features[typeof(IExceptionHandlerFeature)] as IExceptionHandlerFeature;

                    if (error?.Error is SecurityTokenExpiredException)
                    {
                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsync(JsonConvert.SerializeObject(new { authenticated = false, tokenExpired = true }));
                    }
                    else if (error?.Error != null)
                    {
                        context.Response.StatusCode = 500;
                        context.Response.ContentType = "application/json";

                        await context.Response.WriteAsync(
                            JsonConvert.SerializeObject
                            (new { success = false, error = error.Error.Message }));
                    }
                    else
                        await next();
                });
            });

            app.UseJwtBearerAuthentication(new JwtBearerOptions() {
                TokenValidationParameters = new TokenValidationParameters()
                {
                    IssuerSigningKey = key,
                    ValidAudience = tokenOptions.Audience,
                    ValidIssuer = tokenOptions.Issuer,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(0)
                }
            });

            app.UseStaticFiles();

            app.UseMvc();
        }

        private static RSAParameters GetRandomKey()
        {
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                try
                {
                    return rsa.ExportParameters(true);
                }
                finally
                {
                    rsa.PersistKeyInCsp = false;
                }
            }
        }

        public static void Main(string[] args) => new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build()
                .Run();
    }
}
