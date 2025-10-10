using FreeSpin.Application.Common;
using FreeSpin.Application.Common.Interfaces;
using FreeSpin.Application.Exceptions;
using FreeSpin.Application.Spins.Models;
using FreeSpin.Domain.Entities;
using FreeSpin.Domain.Services;
using static FreeSpin.Domain.Common.GlobalConstants;

namespace FreeSpin.Application.Spins;

public class SpinService : ISpinService
{
	private readonly IRepository<Campaign> campaignRepository;
	private readonly IRepository<User> userRepository;
	private readonly IRepository<Spin> spinRepository;
	private readonly IRepository<UserCampaign> userCampaignRepository;
	private readonly IUnitOfWork unitOfWork;
	private readonly IDateTime dateTime;

	public SpinService(
		IRepository<Campaign> campaignRepository,
		IRepository<User> userRepository,
		IRepository<Spin> spinRepository,
		IRepository<UserCampaign> userCampaignRepository,
		IUnitOfWork unitOfWork,
		IDateTime dateTime)
	{
		this.campaignRepository = campaignRepository;
		this.userRepository = userRepository;
		this.spinRepository = spinRepository;
		this.userCampaignRepository = userCampaignRepository;
		this.unitOfWork = unitOfWork;
		this.dateTime = dateTime;
	}

	public async Task<Result<SpinInfoResponse>> GetSpinInfo(int userId, int campaignId)
	{
		var user = userRepository.All().FirstOrDefault(x => x.Id == userId);
		if (user == null)
		{
			return Result<SpinInfoResponse>.Failure("No user found", ErrorType.NotFound);
		}

		var campaign = campaignRepository.All().FirstOrDefault(x => x.Id == campaignId);
		if (campaign == null)
		{
			return Result<SpinInfoResponse>.Failure("No campaign found", ErrorType.NotFound);
		}

		var userCampaign = userCampaignRepository.All()
			.FirstOrDefault(x => x.UserId == user.Id && x.CampaignId == campaign.Id);

		var response = new SpinInfoResponse
		{
			MaxSpins = campaign.MaxSpins,
			SpinUsage = userCampaign?.CurrentSpinCount ?? 0,
			Message = !campaign.IsActive ? "The campaign is no longer active!" : string.Empty
		}; 

		return Result<SpinInfoResponse>.Success(response);
	}

	public async Task<Result<PerformSpinResponse>> PerformSpinAsync(SpinRequest request)
	{
		var user = userRepository.All().FirstOrDefault(x => x.Id == request.UserId);
		if (user == null)
		{
			return Result<PerformSpinResponse>.Failure("No user found", ErrorType.NotFound);
		}

		var campaign = campaignRepository.All().FirstOrDefault(x => x.Id == request.CampaignId);
		if (campaign == null)
		{
			return Result<PerformSpinResponse>.Failure("No campaign found", ErrorType.NotFound);
		}

		if (!campaign.IsActive)
		{
			return Result<PerformSpinResponse>.Failure("The campaign is no longer active.", ErrorType.Forbidden);
		}

		var userCampaign = userCampaignRepository.All()
			.FirstOrDefault(x => x.UserId == user.Id && x.CampaignId == campaign.Id);

		if (userCampaign == null)
		{
			userCampaign = new UserCampaign
			{
				UserId = user.Id,
				CampaignId = campaign.Id,
			};
		}

		if (userCampaign.CurrentSpinCount >= campaign.MaxSpins)
		{
			return Result<PerformSpinResponse>.Failure("Max spins reached for this campaign", ErrorType.Forbidden);
		}

		var spin = new Spin
		{
			UserCampaign = userCampaign,
			Reward = RewardGenerator.GenerateReward(SpinMaxReward, SpinRewardOffset)
		};

		await spinRepository.AddAsync(spin);
		user.Balance += spin.Reward;
		
		// this.userCampaignRepository.Update(userCampaign);
		
		userCampaign.CurrentSpinCount++;

		try
		{
			await unitOfWork.SaveChangesAsync();
		}
		catch (OptimisticConcurrencyException ex)
		{
			if (ex.EntityType == typeof(UserCampaign))
				return Result<PerformSpinResponse>.Failure("Max spins reached due to concurrent request.", ErrorType.Forbidden);

			if (ex.EntityType == typeof(User))
				return Result<PerformSpinResponse>.Failure("Concurrent balance update detected. Retry.", ErrorType.Validation);
		}

		var spins = spinRepository.All()
			.Where(x => x.UserId == user.Id && x.CampaignId == campaign.Id)
			.OrderBy(x => x.CreatedOn)
			.ToList();

		var response = new PerformSpinResponse
		{
			RemainingSpins = campaign.MaxSpins - userCampaign.CurrentSpinCount,
			UserBalance = user.Balance,
			SpinReward = spin.Reward,
			SpinDetails = spins.Select(x => new PerformSpinResponse.SpinDetail
			{
				Reward = x.Reward,
				CreatedOn = x.CreatedOn.ToString("MM/dd/yyyy HH:mm"),
			}).ToList()
		};

		return Result<PerformSpinResponse>.Success(response);
	}
}
