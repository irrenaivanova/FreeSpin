using FreeSpin.Application.Users.Models;
using FreeSpin.Domain.Entities;

namespace FreeSpin.Application.Common.Interfaces;

public interface IUserService
{
	public Task<Result<CreateUserResponse>> CreateUserAsync(CreateUserRequest request);
}
