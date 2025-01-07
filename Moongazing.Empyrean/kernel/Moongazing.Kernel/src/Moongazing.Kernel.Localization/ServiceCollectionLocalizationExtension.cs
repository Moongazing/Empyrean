using Microsoft.Extensions.DependencyInjection;
using Moongazing.Kernel.Localization.Abstractions;
using System.Reflection;

namespace Moongazing.Kernel.Localization;

public static class ServiceCollectionLocalizationExtension
{
    public static IServiceCollection AddYamlResourceLocalization(this IServiceCollection services)
    {
        services.AddScoped<ILocalizationService, ResourceLocalizationService>(_ =>
        {
            Dictionary<string, Dictionary<string, string>> resources = [];

            string[] featureDirs = Directory.GetDirectories(
                Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, "Features")
            );
            foreach (string featureDir in featureDirs)
            {
                string featureName = Path.GetFileName(featureDir);
                string localeDir = Path.Combine(featureDir, "Resources", "Locales");
                if (Directory.Exists(localeDir))
                {
                    string[] localeFiles = Directory.GetFiles(localeDir);
                    foreach (string localeFile in localeFiles)
                    {
                        string localeName = Path.GetFileNameWithoutExtension(localeFile);
                        int separatorIndex = localeName.IndexOf('.');
                        string localeCulture = localeName[(separatorIndex + 1)..];

                        if (File.Exists(localeFile))
                        {
                            if (!resources.ContainsKey(localeCulture))
                                resources.Add(localeCulture, []);
                            resources[localeCulture].Add(featureName, localeFile);
                        }
                    }
                }
            }

            return new ResourceLocalizationService(resources);
        });

        return services;
    }
}