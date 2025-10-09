using FreeSpin.Application.Campaigns.Models;
using FreeSpin.Application.Common.Interfaces;
using FreeSpin.WebAPI.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace FreeSpin.WebAPI.Controllers;

[ApiController]
[Route("campaigns")]
public class CampaignController : ControllerBase
{
	private readonly ICampaignService campaignService;

	public CampaignController(ICampaignService campaignService)
	{
		this.campaignService = campaignService;
	}

	[HttpPost]
	public async Task<IActionResult> Create([FromBody] CreateCampaignRequest request)
	{
		var result = await this.campaignService.CreateCampaignAsync(request);

		if (!result.IsSuccess)
			return result.ToActionResult();

		return Ok(result.Value);
	}

	[HttpGet("{id}")]
	public async Task<IActionResult> GetCampaignById(int id)
	{
		var result = await this.campaignService.GetCampaignByIdAsync(id);

		if (!result.IsSuccess)
		{
			return result.ToActionResult();
		}

		return Ok(result.Value);
	}
}
