using FreeSpin.Application.Common.Interfaces;

namespace FreeSpin.Infrastructure.Services;

public class DateTimeService : IDateTime
{
	public DateTime UtcNow => DateTime.UtcNow;
}
