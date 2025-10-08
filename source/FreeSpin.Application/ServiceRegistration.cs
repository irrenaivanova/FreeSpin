using FreeSpin.Application.Common.Interfaces;
using FreeSpin.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace FreeSpin.Application;

public static class ServiceRegistration
{
	public static IServiceCollection AddApplicationServices(this IServiceCollection services)
	{
		services.AddScoped<IUserService, UserService>();
		services.AddScoped<ICampaignService, CampaignService>();

		return services;
	}
}
