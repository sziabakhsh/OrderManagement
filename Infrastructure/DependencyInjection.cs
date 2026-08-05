using Application.Common.Interfaces;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // 1. read Connection string from config
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            // 2. register main context
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString));

            // 3. connect application db context interface to main context
            services.AddScoped<IAppDbContext>(provider =>
                provider.GetRequiredService<AppDbContext>());

            // for SSRS Report Service
            services.AddHttpClient<ISsrsReportService, SsrsReportService>(client =>
            {
                // initialize HttpClient with base address from configuration
            }).ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
            {
                UseDefaultCredentials = true // using Windows credentials for SSRS authentication
            });

            return services;
        }
    }
}
