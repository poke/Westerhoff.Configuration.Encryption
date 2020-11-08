using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace Westerhoff.Configuration.Encryption
{
    /// <summary>
    /// Configuration protector.
    /// </summary>
    public class ConfigurationProtector : IDisposable
    {
        private readonly Dictionary<string, RSA> _keyCache = new Dictionary<string, RSA>();
        private readonly string _defaultCertificateName;
        private readonly bool _requireValidCertificates;

        /// <summary>
        /// Configuration protector.
        /// </summary>
        /// <param name="defaultCertificateName">Distinguished name of the default certificate.</param>
        /// <param name="requireValidCertificates">Whether to require certificates to be valid.</param>
        public ConfigurationProtector(string defaultCertificateName, bool requireValidCertificates)
        {
            _defaultCertificateName = defaultCertificateName;
            _requireValidCertificates = requireValidCertificates;
        }

        /// <summary>
        /// Try to decrypt a configuration value.
        /// </summary>
        /// <param name="configurationValue">Configuration value.</param>
        /// <param name="decryptedValue">Decrypted value.</param>
        /// <returns>Whether the decryption succeeded.</returns>
        public bool TryDecryptValue(string configurationValue, out string decryptedValue)
        {
            if (!configurationValue.StartsWith(ProtectedConfigurationValue.MarkerPrefix))
            {
                decryptedValue = null;
                return false;
            }

            var value = ProtectedConfigurationValue.Parse(configurationValue);
            var decryptionKey = GetKey(value.CertificateThumbprint.ToString());
            decryptedValue = EncryptionUtility.Decrypt(value.EncryptedValue.ToString(), decryptionKey);
            return true;
        }

        /// <summary>
        /// Get the key for the specified thumbprint
        /// </summary>
        /// <param name="certificateThumbprint">Thumbprint embedded into the configuration value.</param>
        /// <returns></returns>
        private RSA GetKey(string certificateThumbprint)
        {
            if (_keyCache.TryGetValue(certificateThumbprint, out var key))
                return key;

            // load the key from the certificate
            if (certificateThumbprint.Length == 0)
                return _keyCache[string.Empty] = LoadDefaultCertificateKey();
            else
                return _keyCache[certificateThumbprint] = LoadCertificateKey(certificateThumbprint);
        }

        /// <summary>
        /// Load the key from the default certificate in the certificate store.
        /// </summary>
        private RSA LoadDefaultCertificateKey()
        {
            if (string.IsNullOrEmpty(_defaultCertificateName))
                throw new InvalidOperationException("Default certificate is not configured");

            using (var cert = CertificateLoader.LoadCertificateByName(_defaultCertificateName, validOnly: _requireValidCertificates))
            {
                if (cert is null)
                    throw new InvalidOperationException($"Default certificate '{_defaultCertificateName}' was not found in certificate store");
                if (!cert.HasPrivateKey)
                    throw new InvalidOperationException($"Default certificate '{_defaultCertificateName}' has no private key");

                return cert.GetRSAPrivateKey();
            }
        }

        /// <summary>
        /// Load a key from the default certificate in the certificate store.
        /// </summary>
        /// <param name="certificateThumbprint">Certificate thumbprint.</param>
        private RSA LoadCertificateKey(string certificateThumbprint)
        {
            if (string.IsNullOrEmpty(certificateThumbprint))
                throw new ArgumentException("Thumbprint cannot be empty", nameof(certificateThumbprint));

            using (var cert = CertificateLoader.LoadCertificateByThumbprint(certificateThumbprint, validOnly: _requireValidCertificates))
            {
                if (cert is null)
                    throw new InvalidOperationException($"Certificate with thumbprint '{certificateThumbprint}' was not found in certificate store");
                if (!cert.HasPrivateKey)
                    throw new InvalidOperationException($"Certificate with thumbprint '{certificateThumbprint}' has no private key");

                return cert.GetRSAPrivateKey();
            }
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            foreach (var key in _keyCache.Values)
                key.Dispose();
            _keyCache.Clear();
        }
    }
}
