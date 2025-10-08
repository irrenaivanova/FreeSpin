namespace FreeSpin.WebAPI.DTOs.Requests;

public class CreateUserRequest
{
    public string UserName { get; set; } = default!;
    public int Age { get; set; }
}
