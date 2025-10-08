using FreeSpin.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static FreeSpin.Domain.Common.GlobalConstants;

namespace FreeSpin.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
	public void Configure(EntityTypeBuilder<User> builder)
	{
		builder
			.Property(x => x.UserName)
			.HasMaxLength(MaxLengthUserName);

		builder
			.Property(x => x.Balance)
			.HasColumnType("decimal(8,2)");

		builder
			.HasIndex(x => x.UserName).IsUnique();
	}
}
