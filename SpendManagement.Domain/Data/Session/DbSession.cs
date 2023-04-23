using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Data.Session
{
    public class DbSession : IDisposable
    {
        public IDbConnection Connection { get; set; }
        public IConfiguration Configuration { get; private set; }

        public DbSession(IConfiguration configuration)
        {
            this.Configuration = configuration;
            var connection = new SqlConnection(configuration.GetSection("Settings:SqlSettings:ConnectionString").Value);
            Connection = connection;
            Connection.Open();
        }

        public IDbConnection OpenConnection() 
        {
            Connection = new SqlConnection(Configuration.GetSection("Settings:SqlSettings:ConnectionString").Value);
            Connection.Open();
            return Connection;
        }

        public void Dispose() => Connection?.Dispose();
    }
}
