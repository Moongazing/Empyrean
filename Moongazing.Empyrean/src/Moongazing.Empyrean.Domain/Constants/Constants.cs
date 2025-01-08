namespace Moongazing.Empyrean.Domain.Constants;

public static class Constants
{
    public const string PasswordRegex = "^(?=.*[A-Z])(?=.*\\d)(?=.*\\W).{7,20}$";
    public const string PhoneNumberRegex = @"^\+?\d{0,20}$";
}
