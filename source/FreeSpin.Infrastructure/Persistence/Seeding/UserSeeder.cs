using FreeSpin.Domain.Entities;
using FreeSpin.Infrastructure.Persistence.Seeding.Common;

namespace FreeSpin.Infrastructure.Persistence.Seeding;

public class UserSeeder : ISeeder
{
	public async Task SeedAsync(FreeSpinDbContext data, IServiceProvider serviceProvider)
	{
		if (data.Users.Any())
		{
			return;
		}

		var user1 = new User()
		{
			UserName = "User1",
			Age = 20,
			Balance = 200,
		};

		var user2 = new User()
		{
			UserName = "User2",
			Age = 25,
			Balance = 500,
		};

		await data.Users.AddAsync(user1);
		await data.Users.AddAsync(user2);
	}
}
