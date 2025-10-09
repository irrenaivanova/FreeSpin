using FreeSpin.Domain.Common;

namespace FreeSpin.Domain.Entities;

public class UserCampaign : IAuditableEntity
{
	public UserCampaign()
	{
		Spins = new List<Spin>();
	}

	public int UserId { get; set; }

    public User User { get; set; } = default!;

	public int CampaignId { get; set; }

	public Campaign Campaign { get; set; } = default!;

	public IList<Spin> Spins { get; set; }

	public int SpinCount => Spins.Count;

	public DateTime CreatedOn { get; set; }

	public DateTime? ModifiedOn { get; set; }

	public byte[] RowVersion { get; set; } = default!;
}
