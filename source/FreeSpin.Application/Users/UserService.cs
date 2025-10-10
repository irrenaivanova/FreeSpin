using FreeSpin.Application.Common;
using FreeSpin.Application.Common.Interfaces;
using FreeSpin.Application.Users.Models;
using FreeSpin.Domain.Entities;
using static FreeSpin.Domain.Common.GlobalConstants;

namespace FreeSpin.Application.Users
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> userRepository;

        public UserService(IRepository<User> userRepository)
        {
            this.userRepository = userRepository;
        }
        
        public async Task<Result<CreateUserResponse>> CreateUserAsync(CreateUserRequest request)
        {
            if (string.IsNullOrEmpty(request.UserName) || request.UserName.Length > 100)
            {
                return Result<CreateUserResponse>.Failure("Username must not be empty and less 100 characters", ErrorType.Validation);
            }

            if (request.Age < 18 || request.Age >100)
            {
                return Result<CreateUserResponse>.Failure("User must be at between 18 and 100 years old", ErrorType.Validation);
            }

            if (userRepository.AllAsNoTracking().Select(x => x.UserName).Contains(request.UserName))
            {
                return Result<CreateUserResponse>.Failure("A user with this username already exists", ErrorType.Validation);
            }

            var user = new User
            {
                UserName = request.UserName,
                Age = request.Age,
                Balance = StartBalance,
            };
            await userRepository.AddAsync(user);
            await userRepository.SaveChangesAsync();

			var result = new CreateUserResponse
			{
				UserName = user.UserName,
				Balance = user.Balance,
			};

			return Result<CreateUserResponse>.Success(result);
        }
    }
}
