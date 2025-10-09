namespace FreeSpin.Domain.Services;

public static class RewardGenerator
{
	private static readonly Random random = new Random();

	public static decimal GenerateReward(decimal maxReward, decimal offset)
	{
		if (maxReward < offset)
		{
			throw new ArgumentException("MaxReward must be greater than or equal to offset.");
		}

		int maxMultiplier =(int) (maxReward / offset);
		decimal reward = random.Next(1, maxMultiplier + 1) * offset;
		return reward;
	}
}
