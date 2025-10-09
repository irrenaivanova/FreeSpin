using FreeSpin.Domain.Common;

namespace FreeSpin.Domain.Entities;

public class User : BaseModel<int>
{
	public User()
	{
		UserCampaigns = new List<UserCampaign>();
	}

	public string UserName { get; set; } = string.Empty;

    public decimal  Balance { get; set; }

	public int Age { get; set; }

	public IList<UserCampaign> UserCampaigns { get; set; }

	public byte[] RowVersion { get; set; } = default!;
}
