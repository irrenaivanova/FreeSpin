using FreeSpin.Application.Campaigns;
using FreeSpin.Application.Common.Interfaces;
using FreeSpin.Application.Spins;
using FreeSpin.Application.Users;
using Microsoft.Extensions.DependencyInjection;

namespace FreeSpin.Application;

public static class ServiceRegistration
{
	public static IServiceCollection AddApplicationServices(this IServiceCollection services)
	{
		services.AddScoped<IUserService, UserService>();
		services.AddScoped<ICampaignService, CampaignService>();
		services.AddScoped<ISpinService, SpinService>();

		return services;
	}
}
