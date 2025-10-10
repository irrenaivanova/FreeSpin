using FluentAssertions;
using FreeSpin.Application.Campaigns;
using FreeSpin.Application.Campaigns.Models;
using FreeSpin.Application.Common;
using FreeSpin.Application.Common.Interfaces;
using FreeSpin.Domain.Entities;
using Moq;

namespace FreeSpin.Tests.Services;

public class CampaignServiceTests
{
	private readonly Mock<IRepository<Campaign>> campaignRepositoryMock;
	private readonly Mock<IDateTime> dateTimeMock;
	private readonly CampaignService campaignService;
	public CampaignServiceTests()
	{
		this.campaignRepositoryMock = new Mock<IRepository<Campaign>>();
		this.dateTimeMock = new Mock<IDateTime>();
		this.campaignService = new CampaignService(this.campaignRepositoryMock.Object, this.dateTimeMock.Object);
	}


	[Theory]
	[InlineData(0)]
	[InlineData(-5)]
	public async Task CreateCampaignAsync_ShouldReturnFailure_WhenDurationIsInvalid(int invalidDays)
	{
		var request = new CreateCampaignRequest { DurationInDays = invalidDays };

		var result = await this.campaignService.CreateCampaignAsync(request);

		result.IsSuccess.Should().BeFalse();
		result.Error.Should().Contain("positive");
		result.ErrorType.Should().Be(ErrorType.Validation);
	}

	[Fact]
	public async Task CreateCampaignAsync_ShouldReturnSuccess_WhenValid()
	{
		this.campaignRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Campaign>())).Returns(Task.CompletedTask);
		this.campaignRepositoryMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

		var request = new CreateCampaignRequest { DurationInDays = 5 };

		var result = await this.campaignService.CreateCampaignAsync(request);

		result.IsSuccess.Should().BeTrue();
		result.Value.Should().Contain("Successfully started campaign");

		this.campaignRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Campaign>()), Times.Once);
		this.campaignRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
	}

	[Fact]
	public async Task GetCampaignByIdAsync_ShouldReturnFailure_WhenNotFound()
	{
		this.campaignRepositoryMock.Setup(x => x.AllAsNoTracking()).Returns(new List<Campaign>().AsQueryable());

		var result = await this.campaignService.GetCampaignByIdAsync(1);

		result.IsSuccess.Should().BeFalse();
		result.Error.Should().Contain("not found");
		result.ErrorType.Should().Be(ErrorType.NotFound);
	}

	[Fact]
	public async Task GetCampaignByIdAsync_ShouldReturnSuccess_WhenFound()
	{
		var now = DateTime.UtcNow;
		var campaign = new Campaign
		{
			Id = 1,
			DurationInDays = 2,
			MaxSpins = 5,
			CreatedOn = now.AddHours(-1) 
		};
		this.campaignRepositoryMock.Setup(x => x.AllAsNoTracking()).Returns(new List<Campaign> { campaign }.AsQueryable());
		this.dateTimeMock.Setup(x => x.UtcNow).Returns(DateTime.UtcNow);

		var result = await this.campaignService.GetCampaignByIdAsync(1);

		result.IsSuccess.Should().BeTrue();
		result.Value.Should().NotBeNull();
		result.Value.Id.Should().Be(1);
		result.Value.MaxSpins.Should().Be(5);
		result.Value.RemainingHours.Should().BeGreaterThan(0);
	}
}
