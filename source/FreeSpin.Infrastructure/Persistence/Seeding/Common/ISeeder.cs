namespace FreeSpin.Infrastructure.Persistence.Seeding.Common;

public interface ISeeder
{
	Task SeedAsync(FreeSpinDbContext data, IServiceProvider serviceProvider);
}
