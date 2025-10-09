namespace FreeSpin.Application.Users.Models;

public class CreateUserRequest
{
    public string UserName { get; set; } = default!;
    public int Age { get; set; }
}
