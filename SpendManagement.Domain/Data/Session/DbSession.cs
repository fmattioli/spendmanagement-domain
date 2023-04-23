using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Data.Session
{
    public class DbSession : IDisposable
    {
        public IDbConnection Connection { get; private set; }

        public DbSession(IConfiguration configuration)
        {
            var connection = new SqlConnection(configuration.GetSection("Settings:SqlSettings:ConnectionString").Value);
            Connection = connection;
            Connection.Open();
        }

        public void Dispose() => Connection?.Dispose();
    }
}
