using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Template.WebApi.Clean.Infrastructure.Data;
using Template.WebApi.Clean.Infrastructure.Data.Contracts;
using Template.WebApi.Clean.Infrastructure.Data.Models;
using Template.WebApi.Clean.Installers.Contracts;

namespace Template.WebApi.Clean.Installers
{
	public class DbInstaller : IInstallerServiceCollection
	{
		public void InstallServices(IServiceCollection services, IConfiguration configuration)
		{
			// Dapper — lightweight data access
			services.AddScoped<IDbConnection>(_ =>
			{
				var connString = configuration.GetConnectionString("SqlConnectionString");
				return new SqlConnection(connString);
			});

			// EF Core — full ORM with Repository + UnitOfWork pattern
			services.AddDbContextFactory<ApplicationDbContext>(options =>
			{
				var connString = configuration.GetConnectionString("SqlConnectionString");
				options.UseSqlServer(connString);
			});

			// UnitOfWork — scoped per request, no tracking by default
			services.AddScoped<IUnitOfWork<ApplicationDbContext, SampleEfEntity>>(sp =>
			{
				IDbContextFactory<ApplicationDbContext> factory = sp.GetRequiredService<IDbContextFactory<ApplicationDbContext>>();
				return new UnitOfWork<ApplicationDbContext, SampleEfEntity>(factory, tracking: false);
			});
		}
	}
}
