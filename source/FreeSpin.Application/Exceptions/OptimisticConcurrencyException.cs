namespace FreeSpin.Application.Exceptions;

public class OptimisticConcurrencyException : Exception
{
	public Type EntityType { get; }

	public OptimisticConcurrencyException(Type entityType)
	{
		EntityType = entityType;
	}

	public OptimisticConcurrencyException(Type entityType, string message) : base(message)
	{
		EntityType = entityType;
	}
}
