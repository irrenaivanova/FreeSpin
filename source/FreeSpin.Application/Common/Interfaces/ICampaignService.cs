using FreeSpin.Application.Campaigns.Models;

namespace FreeSpin.Application.Common.Interfaces;

public interface ICampaignService
{
	public Task<Result<string>> CreateCampaignAsync(CreateCampaignRequest request);

	public Task<Result<CampaignInfoResponse>> GetCampaignByIdAsync(int id);
}
