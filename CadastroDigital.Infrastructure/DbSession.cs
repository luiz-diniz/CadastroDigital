using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace CadastroDigital.Infrastructure
{
    public sealed class DbSession : IDisposable
    {
        private Guid _id;
        public IDbConnection Connection { get; }
        public IDbTransaction Transaction { get; set; }

        public DbSession(IConfiguration configuration)
        {
            _id = Guid.NewGuid();
            Connection = new SqlConnection(configuration["ConnectionStrings:Default"] ?? throw new ArgumentNullException("Connection String vazia."));
            Connection.Open();
        }

        public void Dispose() => Connection?.Dispose();
    }
}
