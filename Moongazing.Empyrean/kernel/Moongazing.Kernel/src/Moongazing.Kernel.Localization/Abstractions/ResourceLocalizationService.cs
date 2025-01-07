using System.Data;
using System.Text;
using YamlDotNet.RepresentationModel;

namespace Moongazing.Kernel.Localization.Abstractions;

public class ResourceLocalizationService : ILocalizationService
{
    private const string defaultLocale = "en";
    private const string defaultKeySection = "index";
    public ICollection<string>? AcceptLocales { get; set; }

    private readonly Dictionary<string, Dictionary<string, (string path, YamlMappingNode? content)>> resourceData = [];

    public ResourceLocalizationService(Dictionary<string, Dictionary<string, string>> resources)
    {
        foreach ((string locale, Dictionary<string, string> sectionResources) in resources)
        {
            if (!resourceData.ContainsKey(locale))
                resourceData.Add(locale, []);

            foreach ((string sectionName, string sectionResourcePath) in sectionResources)
                resourceData[locale].Add(sectionName, (sectionResourcePath, null));
        }
    }

    public Task<string> GetLocalizedAsync(string key, string? keySection = null)
    {
        return GetLocalizedAsync(key, AcceptLocales ?? throw new NoNullAllowedException(nameof(AcceptLocales)), keySection);
    }

    public Task<string> GetLocalizedAsync(string key, ICollection<string> acceptLocales, string? keySection = null)
    {
        string? localization;
        if (acceptLocales is not null)
            foreach (string locale in acceptLocales)
            {
                localization = GetLocalizationFromResource(key, locale, keySection);
                if (localization is not null)
                    return Task.FromResult(localization);
            }

        localization = GetLocalizationFromResource(key, defaultLocale, keySection);
        if (localization is not null)
            return Task.FromResult(localization);

        return Task.FromResult(key);
    }

    private string? GetLocalizationFromResource(string key, string locale, string? keySection = defaultKeySection)
    {
        if (string.IsNullOrWhiteSpace(keySection))
            keySection = defaultKeySection;

        if (
            resourceData.TryGetValue(locale, out Dictionary<string, (string path, YamlMappingNode? content)>? cultureNode)
            && cultureNode.TryGetValue(keySection, out (string path, YamlMappingNode? content) sectionNode)
        )
        {
            if (sectionNode.content is null)
                LazyLoadResource(sectionNode.path, out sectionNode.content);

            if (sectionNode.content!.Children.TryGetValue(new YamlScalarNode(key), out YamlNode? cultureValueNode))
                return NormalizeString(cultureValueNode.ToString());
        }

        return null;
    }

    private void LazyLoadResource(string path, out YamlMappingNode? content)
    {
        using StreamReader reader = new(path, Encoding.UTF8);
        YamlStream yamlStream = [];
        yamlStream.Load(reader);
        content = (YamlMappingNode)yamlStream.Documents[0].RootNode;
    }
    private static string NormalizeString(string input)
    {
        return input.Normalize(NormalizationForm.FormC);
    }
}