using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
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

            using (var key = GetKey())
            {
                foreach (var config in Data.ToArray())
                {
                    if (config.Value.StartsWith("§#§"))
                    {
                        var value = config.Value.Substring(3);
                        Data[config.Key] = EncryptionUtility.Decrypt(value, key);
                    }
                }
            }
        }

        private RSA GetKey()
        {
            if (string.IsNullOrEmpty(_source.DefaultCertificateName))
                throw new InvalidOperationException("Default certificate is not set");

            using (var cert = CertificateLoader.LoadCertificateFromStore(_source.DefaultCertificateName, validOnly: _source.RequireValidCertificates))
            {
                if (cert is null)
                    throw new InvalidOperationException("Default certificate was not found in certificate store");
                if (!cert.HasPrivateKey)
                    throw new InvalidOperationException("Default certificate has no private key");

                return cert.GetRSAPrivateKey();
            }
        }
    }
}
