using ChatApp.Application.Common.Interfaces;
using ChatApp.Infrastructure.Persistence;
using ChatApp.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ChatApp.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // 1. Database connection
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(connectionString));

            // 2. Repositories mapping (SQL implementations)
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IGlobalChatRepository, GlobalChatRepository>(); 

            // 3. Register SignalR services infrastructure-side
            services.AddSignalR();

            return services;
        }
    }
}