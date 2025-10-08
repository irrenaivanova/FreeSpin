using FreeSpin.Application.Common;
using FreeSpin.Application.Common.Interfaces;
using FreeSpin.Domain.Entities;
using static FreeSpin.Domain.Common.GlobalConstants;

namespace FreeSpin.Application.Services
{
	public class CampaignService : ICampaignService
	{
		private readonly IRepository<Campaign> campaignRepository;

		public CampaignService(IRepository<Campaign> campaignRepository)
		{
			this.campaignRepository = campaignRepository;
		}

		public async Task<Result<Campaign>> CreateCampaignAsync(int durationInDays)
		{
			if (durationInDays <= 0)
			{
				return Result<Campaign>.Failure("The duration of the campaign must be positive", ErrorType.Validation);
			}
			var random = new Random();
			var campaign = new Campaign
			{
				DurationInDays = durationInDays,
				MaxSpins = random.Next(1, MaxSpins + 1)
			};

			await this.campaignRepository.AddAsync(campaign);
			await this.campaignRepository.SaveChangesAsync();
			return Result<Campaign>.Success(campaign);
		}

		public async Task<Result<Campaign>> GetCampaignByIdAsync(int id)
		{
			var campaign = this.campaignRepository.AllAsNoTracking().FirstOrDefault(x => x.Id == id);
			if (campaign == null)
			{
				return Result<Campaign>.Failure("Campaign not found",ErrorType.NotFound);
			}
			return Result<Campaign>.Success(campaign);
		}
	}
}