using FreeSpin.Domain.Common;

namespace FreeSpin.Domain.Entities;

public class Campaign : BaseModel<int>
{
	public Campaign()
	{
		UserCampaigns = new List<UserCampaign>();
	}

	public int DurationInDays { get; set; }

    public int MaxSpins { get; set; }

    public IList<UserCampaign> UserCampaigns { get; set; }

	public bool IsActive => this.CreatedOn.AddHours(this.DurationInDays * 24) > DateTime.UtcNow;
}
