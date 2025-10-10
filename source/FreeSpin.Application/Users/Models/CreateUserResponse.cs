namespace FreeSpin.Application.Users.Models;

public class CreateUserResponse
{
	public string UserName { get; set; } = string.Empty;

	public decimal Balance { get; set; }
}