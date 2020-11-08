using System;

namespace Westerhoff.Configuration.Encryption
{
    /// <summary>
    /// Protected configuration value.
    /// </summary>
    public ref struct ProtectedConfigurationValue
    {
        /// <summary>
        /// Marker prefix used for protected configuration values.
        /// </summary>
        public static readonly string MarkerPrefix = "§#§";

        /// <summary>
        /// Encrypted secret configuration value.
        /// </summary>
        public ReadOnlySpan<char> EncryptedValue
        { get; private set; }

        /// <summary>
        /// Certificate thumbprint.
        /// </summary>
        public ReadOnlySpan<char> CertificateThumbprint
        { get; private set; }

        /// <summary>
        /// Label encoded in the configuration value.
        /// </summary>
        public ReadOnlySpan<char> Label
        { get; private set; }

        /// <summary>
        /// Parse a protected configuration value.
        /// </summary>
        /// <param name="configurationValue">Configuration value.</param>
        /// <returns>Parsed configuration value.</returns>
        public static ProtectedConfigurationValue Parse(string configurationValue)
        {
            if (!configurationValue.StartsWith(MarkerPrefix))
                return default;

            var value = configurationValue.AsSpan().Slice(3);
            var secret = new ProtectedConfigurationValue();

            // parse label
            var labelEndIndex = value.IndexOf('#');
            if (labelEndIndex != -1)
            {
                secret.Label = value.Slice(0, labelEndIndex);
                value = value.Slice(labelEndIndex + 1);
            }

            // parse thumbprint
            var thumbprintEndIndex = value.IndexOf('§');
            if (thumbprintEndIndex != -1)
            {
                secret.CertificateThumbprint = value.Slice(0, thumbprintEndIndex);
                value = value.Slice(thumbprintEndIndex + 1);
            }

            // remainder is the encrypted value
            secret.EncryptedValue = value;

            return secret;
        }
    }
}
