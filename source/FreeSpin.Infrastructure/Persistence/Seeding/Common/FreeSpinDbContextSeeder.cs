namespace FreeSpin.Infrastructure.Persistence.Seeding.Common;

public class FreeSpinDbContextSeeder : ISeeder
{
	public async Task SeedAsync(FreeSpinDbContext data, IServiceProvider serviceProvider)
	{
		ArgumentNullException.ThrowIfNull(data);
		var seeders = new List<ISeeder>
		{
			 new UserSeeder(),
			 new CampaignSeeder(),
		};

		foreach (var seeder in seeders)
		{
			await seeder.SeedAsync(data, serviceProvider);
			await data.SaveChangesAsync(CancellationToken.None);
		}
	}
}
