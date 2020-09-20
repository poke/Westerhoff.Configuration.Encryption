using System;
using Westerhoff.Configuration.Encryption;

namespace Microsoft.Extensions.Configuration
{
    /// <summary>
    /// Extensions for <see cref="IConfigurationBuilder"/> to register
    /// ecnrypted configuration sources.
    /// </summary>
    public static class ConfigurationBuilderExtensions
    {
        /// <summary>
        /// Add a JSON configuration file source that supports encrypted values.
        /// </summary>
        /// <param name="builder">Configuration builder to add the source to.</param>
        /// <param name="path">Relative path to the configuration file.</param>
        /// <param name="defaultCertificateName">The distinguished name of the default certificate to use.</param>
        /// <param name="optional">Whether the file is optional.</param>
        /// <param name="reloadOnChange">Whether to reload the configuration when the file is changed.</param>
        /// <returns>Configuration builder.</returns>
        public static IConfigurationBuilder AddEncryptedJsonFile(this IConfigurationBuilder builder, string path, string defaultCertificateName, bool optional = false, bool reloadOnChange = false)
        {
            if (builder is null)
                throw new ArgumentNullException(nameof(builder));
            if (string.IsNullOrEmpty(path))
                throw new ArgumentException("File path cannot be null or empty", nameof(path));
            if (string.IsNullOrEmpty(defaultCertificateName))
                throw new ArgumentException("Default certificate name cannot be null or empty", nameof(defaultCertificateName));

            return builder.AddEncryptedJsonFile(s =>
            {
                s.DefaultCertificateName = defaultCertificateName;
                s.Path = path;
                s.Optional = optional;
                s.ReloadOnChange = reloadOnChange;
                s.ResolveFileProvider();
            });
        }

        /// <summary>
        /// Add a JSON configuration file source that supports encrypted values.
        /// </summary>
        /// <param name="builder">Configuration builder to add the source to.</param>
        /// <param name="configureSource">Action to configure the <see cref="EncryptedJsonConfigurationSource"/>.</param>
        /// <returns>Configuration builder.</returns>
        public static IConfigurationBuilder AddEncryptedJsonFile(this IConfigurationBuilder builder, Action<EncryptedJsonConfigurationSource> configureSource)
            => builder.Add(configureSource);
    }
}
