using FreeSpin.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace FreeSpin.Infrastructure.Persistence
{
	public class Repository<TEntity> : IRepository<TEntity>
		where TEntity : class
	{
		private readonly FreeSpinDbContext context;
		private readonly DbSet<TEntity> dbSet;
		public Repository(FreeSpinDbContext context)
		{
			this.context = context;
			this.dbSet = context.Set<TEntity>();
		}

		public async Task AddAsync(TEntity entity) => await this.dbSet.AddAsync(entity);

		public IQueryable<TEntity> All() => this.dbSet;

		public IQueryable<TEntity> AllAsNoTracking() => this.dbSet.AsNoTracking();

		public void Delete(TEntity entity) => this.dbSet.Remove(entity);

		public void Dispose() => this.context.Dispose();

		public Task<int> SaveChangesAsync() => this.context.SaveChangesAsync(CancellationToken.None);

		public void Update(TEntity entity) => this.dbSet.Update(entity);
	}
}
