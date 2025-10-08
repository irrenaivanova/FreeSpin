using FreeSpin.Domain.Common;

namespace FreeSpin.Domain.Entities;

public class Spin : BaseModel<string>
{
	public Spin()
	{
		Id = Guid.NewGuid().ToString();
	}

	public decimal Balance { get; set; }

	public UserCampaign UserCampaign { get; set; } = default!;
}
