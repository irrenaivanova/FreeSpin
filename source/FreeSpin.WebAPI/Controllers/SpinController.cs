using FreeSpin.Application.Common.Interfaces;
using FreeSpin.Application.Spins.Models;
using FreeSpin.Application.Users.Models;
using FreeSpin.WebAPI.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace FreeSpin.WebAPI.Controllers;

[ApiController]
[Route("spins")]
public class SpinController : ControllerBase
{
	private readonly ISpinService spinService;

	public SpinController(ISpinService spinService)
	{
		this.spinService = spinService;
	}

	[HttpPost]
	public async Task<IActionResult> Create([FromBody] SpinRequest request)
	{
		var result = await this.spinService.PerformSpinAsync(request);

		if (!result.IsSuccess)
		{
			return result.ToActionResult();
		}

		return Ok(result.Value);
	}

	[HttpGet]
	public async Task<IActionResult> Get([FromQuery] int userId, [FromQuery] int campaignId)
	{
		var result =  await this.spinService.GetSpinInfo(userId, campaignId);

		if (!result.IsSuccess)
		{
			return result.ToActionResult();
		}

		return Ok(result.Value);
	}
}
