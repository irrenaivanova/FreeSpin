using FreeSpin.Application.Common.Interfaces;
using FreeSpin.WebAPI.DTOs.Requests;
using FreeSpin.WebAPI.DTOs.Responses;
using FreeSpin.WebAPI.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace FreeSpin.WebAPI.Controllers;

[ApiController]
[Route("campaigns")]
public class CampaignController : ControllerBase
{
	private readonly ICampaignService campaignService;
	private readonly IDateTime datetime;

	public CampaignController(
		ICampaignService campaignService,
		IDateTime datetime)
	{
		this.campaignService = campaignService;
		this.datetime = datetime;
	}

	[HttpPost]
	public async Task<IActionResult> Create(int durationInDays)
	{
		var result = await this.campaignService.CreateCampaignAsync(durationInDays);

		if (!result.IsSuccess)
			return result.ToActionResult();

		var response = $"Successfully started campaign {result.Value!.Id} at {result.Value!.CreatedOn.ToShortDateString()}";
		return Ok(response);
	}

	[HttpGet("{id}")]
	public async Task<IActionResult> GetCampaignById([FromBody] CreateCampaignRequest request)
	{
		var campaign = await this.campaignService.GetCampaignByIdAsync(request.DurationInDays);

		if (!campaign.IsSuccess)
			return campaign.ToActionResult();

		var response = new CampaignInfoResponse
		{
			Id = campaign.Value!.Id,
			DurationInDays = campaign.Value.DurationInDays,
			MaxSpins = campaign.Value.MaxSpins,
			RemainingHours = (int)Math.Floor((this.datetime.UtcNow - campaign.Value.CreatedOn).TotalHours 
								+ campaign.Value.DurationInDays * 24),
		};

		return Ok(response);
	}
}
