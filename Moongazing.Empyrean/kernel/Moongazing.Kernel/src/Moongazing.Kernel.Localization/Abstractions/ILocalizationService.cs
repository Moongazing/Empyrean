namespace Moongazing.Kernel.Localization.Abstractions;

public interface ILocalizationService
{
    public ICollection<string>? AcceptLocales { get; set; }

    public Task<string> GetLocalizedAsync(string key, string? keySection = null);

    public Task<string> GetLocalizedAsync(string key, ICollection<string> acceptLocales, string? keySection = null);
}