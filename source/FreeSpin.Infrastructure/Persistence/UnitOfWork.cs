using FreeSpin.Application.Common.Interfaces;
using FreeSpin.Application.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace FreeSpin.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
	private readonly FreeSpinDbContext context;

	public UnitOfWork(FreeSpinDbContext context)
	{
		this.context = context;
	}

	public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		try
		{
			await context.SaveChangesAsync(cancellationToken);
		}
		catch (DbUpdateConcurrencyException ex)
		{
			var entry = ex.Entries.FirstOrDefault();
			if (entry != null)
			{
				var entityType = entry.Entity.GetType();
				throw new OptimisticConcurrencyException(entityType, "Concurrency conflict detected.");
			}

			throw new OptimisticConcurrencyException(typeof(object), "Concurrency conflict detected.");
		}
	}
	public void Dispose()
	{
		this.context.Dispose();
	}
}
