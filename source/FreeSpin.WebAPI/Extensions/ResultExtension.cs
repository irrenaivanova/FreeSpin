using FreeSpin.Application.Common;
using Microsoft.AspNetCore.Mvc;

namespace FreeSpin.WebAPI.Extensions;

public static class ResultExtension
{
	public static IActionResult ToActionResult<T>(this Result<T> result)
	{
		if (result.IsSuccess)
		{
			return new OkObjectResult(result.Value);
		}

		return result.ErrorType switch
		{
			ErrorType.NotFound => new NotFoundObjectResult(new { error = result.Error }),
			ErrorType.Forbidden => new ObjectResult(new { error = result.Error })
			{
				StatusCode = StatusCodes.Status403Forbidden
			},
			ErrorType.Validation => new BadRequestObjectResult(new { error = result.Error }),
			_ => new ObjectResult(new { error = result.Error })
			{
				StatusCode = StatusCodes.Status500InternalServerError,
			}
		};
	}
}
