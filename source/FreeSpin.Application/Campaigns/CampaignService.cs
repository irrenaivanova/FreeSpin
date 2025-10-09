using FreeSpin.Application.Campaigns.Models;
using FreeSpin.Application.Common;
using FreeSpin.Application.Common.Interfaces;
using FreeSpin.Domain.Entities;
using static FreeSpin.Domain.Common.GlobalConstants;

namespace FreeSpin.Application.Campaigns
{
    public class CampaignService : ICampaignService
    {
        private readonly IRepository<Campaign> campaignRepository;
		private readonly IDateTime dateTime;

		public CampaignService(
            IRepository<Campaign> campaignRepository,
            IDateTime dateTime)
        {
            this.campaignRepository = campaignRepository;
			this.dateTime = dateTime;
		}

        public async Task<Result<string>> CreateCampaignAsync(CreateCampaignRequest request)
        {
            if (request.DurationInDays <= 0)
            {
                return Result<string>.Failure("The duration of the campaign must be positive", ErrorType.Validation);
            }
            var random = new Random();
            var campaign = new Campaign
            {
                DurationInDays = request.DurationInDays,
                MaxSpins = random.Next(1, MaxSpins + 1)
            };

            await campaignRepository.AddAsync(campaign);
            await campaignRepository.SaveChangesAsync();
			
            var message = $"Successfully started campaign {campaign.Id} at {campaign.CreatedOn.ToShortDateString()}";

			return Result<string>.Success(message);
        }

        public async Task<Result<CampaignInfoResponse>> GetCampaignByIdAsync(int id)
        {
            var campaign = campaignRepository.AllAsNoTracking().FirstOrDefault(x => x.Id == id);
            if (campaign == null)
            {
                return Result<CampaignInfoResponse>.Failure("Campaign not found", ErrorType.NotFound);
            }

			var response = new CampaignInfoResponse
			{
				Id = campaign.Id,
				DurationInDays = campaign.DurationInDays,
				MaxSpins = campaign.MaxSpins,
				RemainingHours = (int)Math.Ceiling(
                    (campaign.CreatedOn.AddHours(campaign.DurationInDays * 24) - this.dateTime.UtcNow).TotalHours)
			};

			return Result<CampaignInfoResponse>.Success(response);
        }
    }
}