using FreeSpin.Application.Common.Interfaces;
using FreeSpin.Application.Users.Models;
using FreeSpin.WebAPI.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace FreeSpin.WebAPI.Controllers;

[ApiController]
[Route("users")]
public class UserController : ControllerBase
{
	private readonly IUserService userService;

	public UserController(IUserService userService)
	{
		this.userService = userService;
	}

	[HttpPost]
	public async Task<IActionResult> Create([FromBody] CreateUserRequest request)
	{
		var result = await this.userService.CreateUserAsync(request);
		
		if (!result.IsSuccess)
		{
			return result.ToActionResult();
		}
			
		return Ok(result.Value);
	}
}
