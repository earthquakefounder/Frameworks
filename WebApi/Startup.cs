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

namespace WebApi
{
    public class Startup
    {
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
            
            services.AddMvc().AddFluentValidation(provider =>
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

            app.UseMvc();
        }
    }
}
