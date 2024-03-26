using Data.Persistence.Interfaces;
using Data.Persistence.UnitOfWork;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using System.Data;

namespace Crosscutting.Extensions
{
    public static class UnitOfWorkExtensions
    {
        public static IServiceCollection AddUnitOfWork(this IServiceCollection services, string connectionString)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped((s) => new NpgsqlConnection(connectionString));
            services.AddScoped<IDbTransaction>(s =>
            {
                NpgsqlConnection conn = s.GetRequiredService<NpgsqlConnection>();
                conn.Open();
                return conn.BeginTransaction();
            });
            return services;
        }
    }
}
