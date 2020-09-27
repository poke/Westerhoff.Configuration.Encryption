namespace Westerhoff.Configuration.Encryption.Utility
{
    /// <summary>
    /// Protected secret.
    /// </summary>
    public class ProtectedSecret
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
        /// Label.
        /// </summary>
        public string Label
        { get; private set; }

        public string SecretText
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Label))
                    return $"§#§{CertificateThumbprint}§{EncryptedValue}";
                else
                    return $"§#§#{Label}#{CertificateThumbprint}§{EncryptedValue}";
            }
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

        /// <summary>
        /// Update the label of this secret.
        /// </summary>
        /// <param name="label">Updated label.</param>
        public void SetLabel(string label)
            => Label = label;

        /// <inheritdoc/>
        public override string ToString()
            => SecretText;
    }
}
