using System;
using System.Security.Cryptography;
using System.Text;

namespace Westerhoff.Configuration.Encryption
{
    /// <summary>
    /// Encryption utility.
    /// </summary>
    public static class EncryptionUtility
    {
        private static readonly RSAEncryptionPadding EncryptionPadding = RSAEncryptionPadding.OaepSHA256;

        /// <summary>
        /// Encrypt a value using a public RSA key.
        /// </summary>
        /// <param name="value">Value to encrypt.</param>
        /// <param name="key">Public RSA key.</param>
        /// <returns>Encrypted value as base64.</returns>
        public static string Encrypt(string value, RSA key)
            => Convert.ToBase64String(key.Encrypt(Encoding.UTF8.GetBytes(value), EncryptionPadding));

        /// <summary>
        /// Decrypt an encrypted value using a private RSA key.
        /// </summary>
        /// <param name="value">Value to decrypt as base64.</param>
        /// <param name="key">Private RSA key.</param>
        /// <returns>Decrypted value.</returns>
        public static string Decrypt(string value, RSA key)
            => Encoding.UTF8.GetString(key.Decrypt(Convert.FromBase64String(value), EncryptionPadding));
    }
}
