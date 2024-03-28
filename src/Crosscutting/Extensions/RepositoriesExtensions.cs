using Data.Persistence.Repository;
using Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Crosscutting.Extensions
{
    public static class RepositoriesExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<ISpendManagementCommandRepository, SpendManagementCommandRepository>();
            services.AddScoped<ISpendManagementEventRepository, SpendManagementEventRepository>();
            services.AddScoped<IVariableReceiptRepository, VariableReceiptRepository>();
            services.AddScoped<IRecurringReceiptRepository, RecurringReceiptRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            return services;
        }
    }
}
