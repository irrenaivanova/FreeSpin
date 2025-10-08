using FreeSpin.Application.Common.Interfaces;
using FreeSpin.Domain.Common;
using FreeSpin.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace FreeSpin.Infrastructure.Persistence;

public class FreeSpinDbContext : DbContext
{
	private readonly IDateTime dateTime;

	public FreeSpinDbContext(
		DbContextOptions<FreeSpinDbContext> options,
		IDateTime dateTime)
	: base(options)
	{
		this.dateTime = dateTime;
	}

	public DbSet<User> Users { get; set; }

	public DbSet<Spin> Spins { get; set; }

	public DbSet<UserCampaign> UserCampaigns { get; set; }

	public DbSet<Campaign> Campaigns { get; set; }

	public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
	{
		foreach (var entry in ChangeTracker.Entries<IAuditableEntity>())
		{
			switch (entry.State)
			{
				case EntityState.Added:
					entry.Entity.CreatedOn = dateTime.UtcNow;
					break;
				case EntityState.Modified:
					entry.Entity.ModifiedOn = dateTime.UtcNow;
					break;
			}
		}

		return base.SaveChangesAsync(cancellationToken);
	}

	protected override void OnModelCreating(ModelBuilder builder)
	{
		base.OnModelCreating(builder);

		builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

		DisableCascadeDeletes(builder);
	}
	private void DisableCascadeDeletes(ModelBuilder builder)
	{
		var entityTypes = builder.Model.GetEntityTypes().ToList();
		var foreignKeys = entityTypes
			.SelectMany(e => e.GetForeignKeys().Where(f => f.DeleteBehavior == DeleteBehavior.Cascade));

		foreach (var foreignKey in foreignKeys)
		{
			foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
		}
	}
}
