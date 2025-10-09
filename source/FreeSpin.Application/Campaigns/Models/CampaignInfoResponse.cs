namespace FreeSpin.Application.Campaigns.Models;

public class CampaignInfoResponse
{
    public int Id { get; set; }
	public int DurationInDays { get; set; }
	public int MaxSpins { get; set; }
	public int RemainingHours { get; set; }
}
