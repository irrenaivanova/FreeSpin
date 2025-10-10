using FluentAssertions;
using FreeSpin.Application.Common.Interfaces;
using FreeSpin.Application.Users;
using FreeSpin.Application.Users.Models;
using FreeSpin.Domain.Entities;
using Moq;
using static FreeSpin.Domain.Common.GlobalConstants;

namespace FreeSpin.Tests.Services;

public class UserServiceTests
{
	private readonly Mock<IRepository<User>> userRepositoryMock;
	private readonly UserService userService;
	public UserServiceTests()
	{
		this.userRepositoryMock = new Mock<IRepository<User>>();
		this.userService = new UserService(this.userRepositoryMock.Object);
	}

	[Fact]
	public async Task CreateUserAsync_ShouldReturnFailure_WhenUserNameIsEmpty()
	{
		var request = new CreateUserRequest { UserName = "", Age = 25 };
		
		var result = await userService.CreateUserAsync(request);
		
		result.IsSuccess.Should().BeFalse();
		result.ErrorType.Should().Be(Application.Common.ErrorType.Validation);
		result.Error.Should().Contain("Username");
	}

	[Theory]
	[InlineData(10)]
	[InlineData(17)]
	[InlineData(101)]
	public async Task CreateUserAsync_ShouldReturnFailure_WhenAgeIsInvalid(int invalidAge)
	{
		var request = new CreateUserRequest { UserName = "Irena", Age = invalidAge };
		
		var result = await this.userService.CreateUserAsync(request);

		result.IsSuccess.Should().BeFalse();
		result.ErrorType.Should().Be(Application.Common.ErrorType.Validation);
		result.Error.Should().Contain("18");
	}

	[Fact]
	public async Task CreateUserAsync_ShouldReturnFailure_WhenUserNameAlreadyExists()
	{
		var existingUsers = new List<User> { new User { UserName = "Irena"} }.AsQueryable();
		this.userRepositoryMock
			.Setup(x => x.AllAsNoTracking())
			.Returns(existingUsers);
		var request = new CreateUserRequest { UserName = "Irena", Age = 25 };
		
		var result = await this.userService.CreateUserAsync(request);

		result.IsSuccess.Should().BeFalse();
		result.ErrorType.Should().Be(Application.Common.ErrorType.Validation);
		result.Error.Should().Contain("already exists");
	}

	[Fact]
	public async Task CreateUserAsync_ShouldReturnSuccess_WhenUserIsValid()
	{
		this.userRepositoryMock
			.Setup (x => x.AllAsNoTracking())
			.Returns(new List<User>().AsQueryable());

		this.userRepositoryMock
			.Setup(x => x.AddAsync(It.IsAny<User>()))
			.Returns(Task.CompletedTask);

		this.userRepositoryMock
			.Setup(x => x.SaveChangesAsync())
			.ReturnsAsync(1);

		var request = new CreateUserRequest { UserName = "Irena", Age = 25 };
		
		var result = await this.userService.CreateUserAsync(request);

		result.IsSuccess.Should().BeTrue();
		result.Value.Should().NotBeNull();
		result.Value!.UserName.Should().Be(request.UserName);
		result.Value.Balance.Should().Be(StartBalance);

		this.userRepositoryMock.Verify(x => x.AddAsync(It.IsAny<User>()), Times.Once);
		this.userRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
	}
}
