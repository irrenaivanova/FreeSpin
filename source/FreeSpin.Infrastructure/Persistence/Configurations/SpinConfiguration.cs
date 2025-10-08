using FreeSpin.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static FreeSpin.Domain.Common.GlobalConstants;

namespace FreeSpin.Infrastructure.Persistence.Configurations;

public class SpinConfiguration : IEntityTypeConfiguration<Spin>
{
	public void Configure(EntityTypeBuilder<Spin> builder)
	{
		builder
			.HasOne(x => x.UserCampaign)
			.WithMany(x => x.Spins)
			.HasForeignKey(x => new {x.UserId, x.CampaignId});

		builder
			.Property(x => x.Reward)
			.HasColumnType("decimal(8,2)");

		builder
			.Property(x => x.Id)
			.HasMaxLength(MaxLengthGuid);
	}
}
