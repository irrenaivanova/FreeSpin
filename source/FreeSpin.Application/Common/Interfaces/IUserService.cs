using FreeSpin.Domain.Entities;

namespace FreeSpin.Application.Common.Interfaces;

public interface IUserService
{
	public Task<Result<User>> CreateUserAsync(string userName, int age);
}
