namespace FreeSpin.Application.Common.Interfaces;

public interface IUnitOfWork : IDisposable
{
	Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
