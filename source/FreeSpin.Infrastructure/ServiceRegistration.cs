using FreeSpin.Application.Common.Interfaces;
using FreeSpin.Infrastructure.Persistence;
using FreeSpin.Infrastructure.Persistence.Seeding.Common;
using FreeSpin.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FreeSpin.Infrastructure;

public static class ServiceRegistration
{
	public static IServiceCollection AddInfrastructure(
			this IServiceCollection services,
			IConfiguration configuration)
	{
		services.AddDbContext<FreeSpinDbContext>(options =>
			options.UseSqlServer(
				configuration.GetConnectionString("DefaultConnection"),
				b => b.MigrationsAssembly(typeof(FreeSpinDbContext).Assembly.FullName)));

		services.AddTransient<IDateTime, DateTimeService>();
		services.AddTransient<FreeSpinDbContextSeeder>();
		services.AddTransient<ISeeder, FreeSpinDbContextSeeder>();
		services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
		services.AddScoped<IUnitOfWork, UnitOfWork>();

		return services;
	}
}
