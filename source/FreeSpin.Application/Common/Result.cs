namespace FreeSpin.Application.Common;

public class Result<T>
{
	private Result(bool isSuccess, T? value, string? error, ErrorType errorType)
	{
		IsSuccess = isSuccess;
		Value = value;
		Error = error;
		ErrorType = errorType;
	}
	public bool IsSuccess { get; }
	public string? Error { get; }
	public ErrorType ErrorType { get; }
	public T? Value { get; }

	public static Result<T> Success(T value)
		=> new Result<T>(true, value, null, ErrorType.None);

	public static Result<T> Failure(string error, ErrorType errorType)
		=> new Result<T>(false,default, error, errorType);
}

public enum ErrorType
{
	None,
	NotFound,
	Forbidden,
	Validation
}