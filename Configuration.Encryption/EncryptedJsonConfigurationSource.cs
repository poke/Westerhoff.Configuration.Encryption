using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace Westerhoff.Configuration.Encryption
{
    /// <summary>
    /// Configuration source for a JSON configuration file that supports
    /// encrypted configuration values.
    /// </summary>
    public class EncryptedJsonConfigurationSource : JsonConfigurationSource
    {
        /// <summary>
        /// The name of the certificate to use as default when no inline
        /// certificate is specified with the configuration value.
        /// </summary>
        public string DefaultCertificateName
        { get; set; }

        /// <summary>
        /// Whether to require certificates to be valid.
        /// </summary>
        public bool RequireValidCertificates
        { get; set; } = true;

        /// <summary>
        /// Builds the <see cref="EncryptedJsonConfigurationProvider"/> for this source.
        /// </summary>
        /// <param name="builder">The configuration builder.</param>
        /// <returns>A <see cref="EncryptedJsonConfigurationProvider"/>.</returns>
        public override IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            EnsureDefaults(builder);
            return new EncryptedJsonConfigurationProvider(this);
        }
    }
}
