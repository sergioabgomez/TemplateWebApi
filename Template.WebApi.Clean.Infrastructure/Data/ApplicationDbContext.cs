using Microsoft.EntityFrameworkCore;
using Template.WebApi.Clean.Domain.Models;
using Template.WebApi.Clean.Infrastructure.Data.Models;

namespace Template.WebApi.Clean.Infrastructure.Data
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

		public DbSet<SampleEntity> Samples => Set<SampleEntity>();
		public DbSet<SampleEfEntity> EfSamples => Set<SampleEfEntity>();

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<SampleEntity>(entity =>
			{
				entity.ToTable("Samples");
				entity.HasKey(e => e.Id);
				entity.Property(e => e.Name).HasMaxLength(200).IsRequired();
				entity.Property(e => e.Description).HasMaxLength(1000);
			});

			modelBuilder.Entity<SampleEfEntity>(entity =>
			{
				entity.ToTable("EfSamples");
				entity.HasKey(e => e.Id);
				entity.Property(e => e.Name).HasMaxLength(200).IsRequired();
				entity.Property(e => e.Description).HasMaxLength(1000);
			});
		}
	}
}
