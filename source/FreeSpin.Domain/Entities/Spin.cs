using FreeSpin.Domain.Common;

namespace FreeSpin.Domain.Entities;

public class Spin : BaseModel<string>
{
	public Spin()
	{
		Id = Guid.NewGuid().ToString();
	}

	public decimal Reward { get; set; }

	public int UserId { get; set; }

	public int CampaignId { get; set; }

	public UserCampaign UserCampaign { get; set; } = default!;
}
