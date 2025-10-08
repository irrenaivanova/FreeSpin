using FreeSpin.Application.Common.Interfaces;
using FreeSpin.Application.Services;
using FreeSpin.WebAPI.DTOs.Requests;
using FreeSpin.WebAPI.DTOs.Responses;
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
		var result = await this.userService.CreateUserAsync(request.UserName, request.Age);
		
		if (!result.IsSuccess)
			return result.ToActionResult();

		var response = new CreateUserResponse
		{
			UserName = result.Value!.UserName,
			Balance = result.Value.Balance,
		};
		return Ok(response);
	}
}
