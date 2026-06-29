using Microsoft.Data.SqlClient;
using System.Data;
using Template.WebApi.Clean.Installers.Contracts;

namespace Template.WebApi.Clean.Installers
{
    public class DbInstaller : IInstallerServiceCollection
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IDbConnection>(_ =>
            {
                var connString = configuration.GetConnectionString("SqlConnectionString");
                return new SqlConnection(connString);
            });
        }
    }
}
