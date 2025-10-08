using FreeSpin.Application.Common.Interfaces;

namespace FreeSpin.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
	private readonly FreeSpinDbContext context;

	public UnitOfWork(FreeSpinDbContext context)
	{
		this.context = context;
	}

	public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
			=> this.context.SaveChangesAsync(cancellationToken);

	public void Dispose()
	{
		this.context.Dispose();
	}
}
