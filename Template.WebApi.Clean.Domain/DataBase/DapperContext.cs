using Microsoft.Data.SqlClient;
using System.Data;
namespace Template.WebApi.Clean.Domain.DataBase
{
    public class DapperContext : IDisposable
    {
        private string _connectionString { get; }
        public DapperContext(string connectionString) { _connectionString = connectionString; }
        public IDbConnection Connection => new SqlConnection(_connectionString);
        public void Dispose() { Connection.Dispose(); }
    }
}
