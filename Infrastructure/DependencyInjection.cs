using Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

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

            return services;
        }
    }
}
