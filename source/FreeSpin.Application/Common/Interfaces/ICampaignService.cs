using FreeSpin.Domain.Entities;

namespace FreeSpin.Application.Common.Interfaces;

public interface ICampaignService
{
	public Task<Result<Campaign>> CreateCampaignAsync(int durationInDays);

	public Task<Result<Campaign>> GetCampaignByIdAsync(int id);
}
