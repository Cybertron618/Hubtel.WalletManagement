using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StackExchange.Redis;
using Microsoft.EntityFrameworkCore;
using Hubtel.WalletManagement.Api.Data;
using Hubtel.WalletManagement.Api.Services;
using Hubtel.WalletManagement.Api.Repositories;
using Hubtel.WalletManagement.Api.Interfaces;
using Hubtel.WalletManagement.Api.Operations;


namespace Hubtel.WalletManagement.Api
{
    public class Startup(IConfiguration configuration)
    {
        public IConfiguration Configuration { get; } = configuration 
            ?? throw new ArgumentNullException(nameof(configuration));

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<WalletDbContext>(options =>
        options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));

            string redisConnectionString = Configuration.GetConnectionString("Redis") 
                ?? throw new InvalidOperationException("Redis connection string is missing or invalid.");

            services.AddSingleton(ConnectionMultiplexer.Connect(redisConnectionString));

            //other services and configurations
            services.AddTransient<IWalletService, WalletService>();
            services.AddTransient<IWalletOperations, WalletOperations>();
            services.AddTransient<IWalletRepository, WalletRepository>();
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            ArgumentNullException.ThrowIfNull(env);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error");
                app.UseHsts();
            }
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
