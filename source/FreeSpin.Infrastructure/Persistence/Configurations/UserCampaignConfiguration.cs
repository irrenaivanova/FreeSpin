using FreeSpin.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FreeSpin.Infrastructure.Persistence.Configurations;

public class UserCampaignConfiguration : IEntityTypeConfiguration<UserCampaign>
{
	public void Configure(EntityTypeBuilder<UserCampaign> builder)
	{
		builder
			.HasKey(x => new {x.UserId, x.CampaignId});

		builder
			.HasOne(x => x.User)
			.WithMany(x => x.UserCampaigns)
			.HasForeignKey(x => x.UserId);

		builder
			.HasOne(x => x.Campaign)
			.WithMany(x => x.UserCampaigns)
			.HasForeignKey(x => x.CampaignId);

		builder
			.Property(x => x.RowVersion)
			.IsRowVersion()
			.IsConcurrencyToken();
	}
}
