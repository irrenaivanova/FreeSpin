using FluentAssertions;
using FreeSpin.Application.Common;
using FreeSpin.Application.Common.Interfaces;
using FreeSpin.Application.Spins;
using FreeSpin.Application.Spins.Models;
using FreeSpin.Domain.Entities;
using Moq;

namespace FreeSpin.Tests.Services;

public class SpinServiceTests
{
	private readonly Mock<IRepository<User>> userRepoMock;
	private readonly Mock<IRepository<Campaign>> campaignRepoMock;
	private readonly Mock<IRepository<UserCampaign>> userCampaignRepoMock;
	private readonly Mock<IRepository<Spin>> spinRepoMock;
	private readonly Mock<IUnitOfWork> unitOfWorkMock;
	private readonly Mock<IDateTime> dateTimeMock;
	private readonly SpinService spinService;

	public SpinServiceTests()
	{
		this.userRepoMock = new Mock<IRepository<User>>();
		this.campaignRepoMock = new Mock<IRepository<Campaign>>();
		this.userCampaignRepoMock = new Mock<IRepository<UserCampaign>>();
		this.spinRepoMock = new Mock<IRepository<Spin>>();
		this.unitOfWorkMock = new Mock<IUnitOfWork>();
		this.dateTimeMock = new Mock<IDateTime>();

		this.spinService = new SpinService(
				this.campaignRepoMock.Object,
				this.userRepoMock.Object,
				this.spinRepoMock.Object,
				this.userCampaignRepoMock.Object,
				this.unitOfWorkMock.Object,
				this.dateTimeMock.Object
			);
	}

	[Fact]
	public async Task GetSpinInfo_ShouldReturnNotFound_WhenUserDoesNotExist()
	{
		this.userRepoMock.Setup(x => x.All()).Returns(new List<User>().AsQueryable());
		this.campaignRepoMock.Setup(x => x.All()).Returns(new List<Campaign>().AsQueryable());

		var result = await this.spinService.GetSpinInfo(1, 1);

		result.IsSuccess.Should().BeFalse();
		result.ErrorType.Should().Be(ErrorType.NotFound);
		result.Error.Should().Contain("No user found");
	}

	[Fact]
	public async Task GetSpinInfo_ShouldReturnNotFound_WhenCampaignDoesNotExist()
	{
		var user = new User { Id = 1 };
		this.userRepoMock.Setup(r => r.All()).Returns(new List<User> { user }.AsQueryable());
		this.campaignRepoMock.Setup(r => r.All()).Returns(new List<Campaign>().AsQueryable());

		var result = await this.spinService.GetSpinInfo(1, 1);

		result.IsSuccess.Should().BeFalse();
		result.ErrorType.Should().Be(ErrorType.NotFound);
		result.Error.Should().Contain("No campaign found");
	}

	[Fact]
	public async Task PerformSpinAsync_ShouldIncrementUntilMaxReached()
	{
		var user = new User { Id = 1, Balance = 100m };
		var campaign = new Campaign { Id = 1, MaxSpins = 3, CreatedOn = DateTime.UtcNow, DurationInDays = 3 };
		var userCampaign = new UserCampaign { UserId = 1, CampaignId = 1, CurrentSpinCount = 0 };

		this.userRepoMock.Setup(x => x.All()).Returns(new List<User> { user }.AsQueryable());
		this.campaignRepoMock.Setup(x => x.All()).Returns(new List<Campaign> { campaign }.AsQueryable());
		this.userCampaignRepoMock.Setup(x => x.All()).Returns(new List<UserCampaign> { userCampaign }.AsQueryable());
		this.spinRepoMock.Setup(x => x.AddAsync(It.IsAny<Spin>())).Returns(Task.CompletedTask);

		for (int i = 0; i < campaign.MaxSpins; i++)
		{
			var result = await this.spinService.PerformSpinAsync(new SpinRequest { UserId = user.Id, CampaignId = campaign.Id });
			result.IsSuccess.Should().BeTrue();
			userCampaign.CurrentSpinCount.Should().Be(i + 1);
		}

		var forbidden = await this.spinService.PerformSpinAsync(new SpinRequest { UserId = user.Id, CampaignId = campaign.Id });
		forbidden.IsSuccess.Should().BeFalse();
		forbidden.ErrorType.Should().Be(ErrorType.Forbidden);
		forbidden.Error.Should().Contain("Max spins reached");
	}

	[Fact]
	public async Task PerformSpinAsync_ShouldReturnForbidden_WhenCampaignInactive()
	{
		var user = new User { Id = 1 };
		var campaign = new Campaign { Id = 1, MaxSpins = 3, CreatedOn = DateTime.UtcNow.AddDays(-2), DurationInDays = 1 };

		this.userRepoMock.Setup(r => r.All()).Returns(new List<User> { user }.AsQueryable());
		this.campaignRepoMock.Setup(r => r.All()).Returns(new List<Campaign> { campaign }.AsQueryable());

		var result = await this.spinService.PerformSpinAsync(new SpinRequest { UserId = user.Id, CampaignId = campaign.Id });

		result.IsSuccess.Should().BeFalse();
		result.ErrorType.Should().Be(ErrorType.Forbidden);
		result.Error.Should().Contain("no longer active");
	}

	[Fact]
	public async Task PerformSpinAsync_ShouldNotExceedMaxUnderParallelCalls()
	{
		var user = new User { Id = 1, Balance = 100m };
		var campaign = new Campaign { Id = 1, MaxSpins = 5, CreatedOn = DateTime.UtcNow, DurationInDays = 3 };
		var userCampaign = new UserCampaign { UserId = 1, CampaignId = 1, CurrentSpinCount = 0 };

		this.userRepoMock.Setup(x => x.All()).Returns(new List<User> { user }.AsQueryable());
		this.campaignRepoMock.Setup(x => x.All()).Returns(new List<Campaign> { campaign }.AsQueryable());
		this.userCampaignRepoMock.Setup(x => x.All()).Returns(new List<UserCampaign> { userCampaign }.AsQueryable());
		this.spinRepoMock.Setup(x => x.AddAsync(It.IsAny<Spin>())).Returns(Task.CompletedTask);

		var tasks = Enumerable.Range(0, 10)
			.Select(x => this.spinService.PerformSpinAsync(new SpinRequest { UserId = user.Id, CampaignId = campaign.Id }));

		var results = await Task.WhenAll(tasks);

		var successCount = results.Count(x => x.IsSuccess);
		var forbiddenCount = results.Count(x => !x.IsSuccess && x.ErrorType == ErrorType.Forbidden);

		successCount.Should().Be(campaign.MaxSpins);
		forbiddenCount.Should().Be(10 - campaign.MaxSpins);
		userCampaign.CurrentSpinCount.Should().Be(campaign.MaxSpins);
	}
}
