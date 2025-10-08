using FreeSpin.Application.Common;
using FreeSpin.Application.Common.Interfaces;
using FreeSpin.Domain.Entities;
using static FreeSpin.Domain.Common.GlobalConstants;

namespace FreeSpin.Application.Services
{
	public class UserService : IUserService
	{
		private readonly IRepository<User> userRepository;

		public UserService(IRepository<User> userRepository)
		{
			this.userRepository = userRepository;
		}

		public async Task<Result<User>> CreateUserAsync(string userName, int age)
		{
			if (string.IsNullOrEmpty(userName) || userName.Length > 100)
			{
				return Result<User>.Failure("Username must not be empty and less 100 characters", ErrorType.Validation);
			}

			if (age < 18)
			{
				return Result<User>.Failure("User must be at least 18 years old", ErrorType.Validation);
			}

			if (this.userRepository.AllAsNoTracking().Select(x => x.UserName).Contains(userName))
			{
				return Result<User>.Failure("A user with this username already exists", ErrorType.Validation);
			}

			var user = new User
			{
				UserName = userName,
				Age = age,
				Balance = StartBalance,
			};
			await this.userRepository.AddAsync(user);
			await this.userRepository.SaveChangesAsync();

			return Result<User>.Success(user);
		}
	}
}
