using System.Text.Json.Serialization;

namespace Moongazing.Kernel.Application.Dtos;

public class UserRegisterDto : IDto
{
    public required string Email { get; set; }

    [JsonIgnore]
    public string Password { get; set; }
    [JsonIgnore]
    public string RepeatPassword { get; set; }

    public UserRegisterDto()
    {
        Email = string.Empty;
        Password = string.Empty;
        RepeatPassword = string.Empty;
    }

    public UserRegisterDto(string email, string password, string repeatPassword)
    {
        Email = email;
        Password = password;
        RepeatPassword = repeatPassword;
    }
}