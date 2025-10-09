namespace FreeSpin.Application.Spins.Models;

public class SpinResponse
{
	public int RemainingSpins { get; set; }
	public List<SpinDetail> SpinDetails { get; set; } = new List<SpinDetail>();

	public int TotalSpins => SpinDetails.Count;

	public class SpinDetail
	{
		public DateTime CreatedOn { get; set; }

		public decimal Balance { get; set; }
	}
}

