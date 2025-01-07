using System.Text.Json.Serialization;

namespace Moongazing.Kernel.Application.Dtos;

public class UserLoginDto : IDto
{
    public required string Email { get; set; }

    [JsonIgnore]
    public string Password { get; set; }

    [JsonIgnore]
    public string? AuthenticatorCode { get; set; }

    public UserLoginDto()
    {
        Email = string.Empty;
        Password = string.Empty;
    }

    public UserLoginDto(string email, string password)
    {
        Email = email;
        Password = password;
    }
}