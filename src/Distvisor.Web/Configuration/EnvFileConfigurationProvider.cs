using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.IO;

namespace Distvisor.Web.Configuration
{
    public class EnvFileConfigurationProvider : FileConfigurationProvider
    {
        public EnvFileConfigurationProvider(EnvFileConfigurationSource source) : base(source) { }

        public override void Load(Stream stream)
        {
            var data = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            var sectionPrefix = ((EnvFileConfigurationSource)Source).SectionName + ConfigurationPath.KeyDelimiter;

            using (var reader = new StreamReader(stream))
            {
                while (reader.Peek() != -1)
                {
                    var rawLine = reader.ReadLine();
                    var line = rawLine.Trim();

                    // Ignore blank lines
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        continue;
                    }

                    // key = value
                    int separator = line.IndexOf('=');
                    if (separator < 0)
                    {
                        throw new FormatException($"FormatError_UnrecognizedLineFormat {rawLine}");
                    }

                    string key = sectionPrefix + line.Substring(0, separator).Trim();
                    string value = line.Substring(separator + 1).Trim();

                    // Remove dashes
                    key = key.Replace("_", string.Empty);

                    if (data.ContainsKey(key))
                    {
                        throw new FormatException($"FormatError_KeyIsDuplicated {key}");
                    }

                    data[key] = value;
                }
            }

            Data = data;
        }
    }

    public class EnvFileConfigurationSource : FileConfigurationSource
    {
        public string SectionName { get; set; }

        public override IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            EnsureDefaults(builder);
            return new EnvFileConfigurationProvider(this);
        }
    }

    public static class EnvFileConfigurationExtensions
    {
        public static IConfigurationBuilder AddEnvFile(this IConfigurationBuilder builder, string path, string sectionName, bool optional)
        {
            return AddEnvFile(builder, provider: null, path: path, sectionName: sectionName, optional: optional, reloadOnChange: false);
        }

        public static IConfigurationBuilder AddEnvFile(this IConfigurationBuilder builder, IFileProvider provider, string path, string sectionName, bool optional, bool reloadOnChange)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException("Error_InvalidFilePath", nameof(path));
            }
            if (string.IsNullOrEmpty(sectionName))
            {
                throw new ArgumentException("Error_InvalidSectionName", nameof(path));
            }

            return builder.AddEnvFile(s =>
            {
                s.FileProvider = provider;
                s.Path = path;
                s.SectionName = sectionName;
                s.Optional = optional;
                s.ReloadOnChange = reloadOnChange;
                s.ResolveFileProvider();
            });
        }

        public static IConfigurationBuilder AddEnvFile(this IConfigurationBuilder builder, Action<EnvFileConfigurationSource> configureSource)
            => builder.Add(configureSource);
    }
}
