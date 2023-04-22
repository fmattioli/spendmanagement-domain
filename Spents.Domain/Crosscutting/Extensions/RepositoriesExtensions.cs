using Data.Persistence;
using Data.Session;
using Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Crosscutting.Extensions
{
    public static class RepositoriesExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<DbSession>();
            services.AddTransient<IReceiptRepository, ReceiptRepository>();
            return services;
        }
    }
}
