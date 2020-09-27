using System;
using System.Security.Cryptography;
using System.Text;

namespace Westerhoff.Configuration.Encryption.Utility
{
    /// <summary>
    /// Configuration protector.
    /// </summary>
    public class ConfigurationProtector
    {
        /// <summary>
        /// Protect a configuration value using a certificate.
        /// </summary>
        /// <param name="value">Secret configuration value to protect.</param>
        /// <param name="certificate">Certificate to protect the value with.</param>
        /// <returns>Protected configuration value.</returns>
        public ProtectedSecret Protect(string value, CertificateInfo certificate)
        {
            var key = certificate.EncryptionKey;
            if (key is null)
                throw new ArgumentException("Certificate information was disposed", nameof(certificate));

            var encryptedValue = Encrypt(value, key);
            return new ProtectedSecret(encryptedValue, certificate.Thumbprint);
        }

        /// <summary>
        /// Encrypt a value using a public RSA key.
        /// </summary>
        /// <param name="value">Value to encrypt.</param>
        /// <param name="key">Public RSA key.</param>
        /// <returns>Encrypted value as base64.</returns>
        private static string Encrypt(string value, RSA key)
            => Convert.ToBase64String(key.Encrypt(Encoding.UTF8.GetBytes(value), RSAEncryptionPadding.OaepSHA256));
    }
}
