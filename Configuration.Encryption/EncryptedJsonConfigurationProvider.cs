using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration.Json;

namespace Westerhoff.Configuration.Encryption
{
    /// <summary>
    /// Configuration provider for a JSON configuration file that supports
    /// encrypted configuration values.
    /// </summary>
    public class EncryptedJsonConfigurationProvider : JsonConfigurationProvider
    {
        private readonly EncryptedJsonConfigurationSource _source;

        /// <summary>
        /// Create a new configuration provider for the specified source.
        /// </summary>
        /// <param name="source">Configuration source.</param>
        public EncryptedJsonConfigurationProvider(EncryptedJsonConfigurationSource source)
            : base(source)
        {
            _source = source;
        }

        /// <inheritdoc/>
        public override void Load(Stream stream)
        {
            base.Load(stream);

            using (var protector = new ConfigurationProtector(_source.DefaultCertificateName, _source.RequireValidCertificates))
            {
                foreach (var config in Data.ToArray())
                {
                    if (protector.TryDecryptValue(config.Value, out var decryptedValue))
                        Data[config.Key] = decryptedValue;
                }
            }
        }
    }
}
