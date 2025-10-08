using FreeSpin.Domain.Entities;
using FreeSpin.Infrastructure.Persistence.Seeding.Common;

namespace FreeSpin.Infrastructure.Persistence.Seeding
{
	public class CampaignSeeder : ISeeder
	{
		public async Task SeedAsync(FreeSpinDbContext data, IServiceProvider serviceProvider)
		{
			if (data.Campaigns.Any())
			{
				return;
			}

			var campaign1 = new Campaign()
			{
				DurationInDays = 2,
				MaxSpins = 3,
			};

			var campaign2 = new Campaign()
			{
				DurationInDays = 1,
				MaxSpins = 5,
			};

			await data.Campaigns.AddAsync(campaign1);
			await data.Campaigns.AddAsync(campaign2);
		}
	}
}
