namespace FreeSpin.Application.Spins.Models;

public class PerformSpinResponse
{
	public int SpinUsage => SpinDetails.Count;

	public int RemainingSpins { get; set; }

	public decimal SpinReward { get; set; }

	public decimal UserBalance { get; set; }

	public List<SpinDetail> SpinDetails { get; set; } = new List<SpinDetail>();

	public class SpinDetail
	{
		public string CreatedOn { get; set; } = string.Empty;

		public decimal Reward { get; set; }
	}
}
