using FreeSpin.Application.Spins.Models;

namespace FreeSpin.Application.Common.Interfaces;

public interface ISpinService
{
	public Task<Result<PerformSpinResponse>> PerformSpinAsync(SpinRequest request);

	public Task<Result<SpinInfoResponse>> GetSpinInfo(int userId, int campaignId);
}
