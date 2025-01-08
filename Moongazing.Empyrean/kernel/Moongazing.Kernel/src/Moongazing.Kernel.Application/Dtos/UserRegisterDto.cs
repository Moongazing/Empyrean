using System.Text.Json.Serialization;

namespace Moongazing.Kernel.Application.Dtos;

public class UserRegisterDto : IDto
{
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }

    [JsonIgnore]
    public string Password { get; set; }
    [JsonIgnore]
    public string RepeatPassword { get; set; }

    public UserRegisterDto()
    {
        Email = string.Empty;
        Password = string.Empty;
        RepeatPassword = string.Empty;
        FirstName = string.Empty;
        LastName = string.Empty;
    }

    public UserRegisterDto(string email, string password, string repeatPassword, string firstName, string lastName)
    {
        Email = email;
        Password = password;
        RepeatPassword = repeatPassword;
        FirstName = firstName;
        LastName = lastName;
    }
}