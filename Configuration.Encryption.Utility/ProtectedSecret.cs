using System.Text;

namespace Westerhoff.Configuration.Encryption.Utility
{
    /// <summary>
    /// Protected secret.
    /// </summary>
    public sealed class ProtectedSecret
    {
        /// <summary>
        /// Encrypted secret configuration value.
        /// </summary>
        public string EncryptedValue
        { get; }

        /// <summary>
        /// Certificate thumbprint.
        /// </summary>
        public string CertificateThumbprint
        { get; }

        /// <summary>
        /// Get the configuration value for the protected secret.
        /// </summary>
        /// <param name="label">Optional label to include in the configuration value.</param>
        /// <param name="includeCertThumbprint">Whether to include the certificate thumbprint.</param>
        /// <returns>Combined configuration value for the protected secret.</returns>
        public string GetConfigurationValue(string label = null, bool includeCertThumbprint = true)
        {
            var value = new StringBuilder("§#§");

            if (!string.IsNullOrWhiteSpace(label))
                value.Append($"{label.Replace("#", "").Replace("§", "")}#");

            if (includeCertThumbprint)
                value.Append($"{CertificateThumbprint}§");

            value.Append(EncryptedValue);
            return value.ToString();
        }

        /// <summary>
        /// Create a new protected secret.
        /// </summary>
        /// <param name="encryptedValue">Encrypted secret value.</param>
        /// <param name="certificateThumbprint">Certificate thumbprint.</param>
        public ProtectedSecret(string encryptedValue, string certificateThumbprint)
        {
            EncryptedValue = encryptedValue;
            CertificateThumbprint = certificateThumbprint;
        }

        /// <inheritdoc/>
        public override string ToString()
            => GetConfigurationValue();
    }
}
