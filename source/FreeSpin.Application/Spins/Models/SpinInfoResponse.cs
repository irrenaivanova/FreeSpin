using System.Diagnostics;

namespace FreeSpin.Application.Spins.Models;

public class SpinInfoResponse
{
	public int SpinUsage { get; set; }

	public int MaxSpins { get; set; }

	public string? Message { get; set; }
}