using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace Westerhoff.Configuration.Encryption.Utility
{
    /// <summary>
    /// Certificate information.
    /// </summary>
    public class CertificateInfo : IDisposable
    {
        /// <summary>
        /// Certificate label.
        /// </summary>
        public string Label
        { get; }

        /// <summary>
        /// Subject.
        /// </summary>
        public string Subject
        { get; }

        /// <summary>
        /// Certificate thumbprint.
        /// </summary>
        public string Thumbprint
        { get; }

        /// <summary>
        /// Encryption key.
        /// </summary>
        public RSA EncryptionKey
        { get; private set; }

        /// <summary>
        /// Create a new certificate information from a certificate.
        /// </summary>
        /// <param name="certificate">Certificate.</param>
        /// <remarks>The certificate will not be referenced, so it can be safely disposed.</remarks>
        public CertificateInfo(X509Certificate2 certificate)
        {
            EncryptionKey = certificate.GetRSAPublicKey();
            Subject = certificate.Subject;
            Thumbprint = certificate.Thumbprint;

            if (!string.IsNullOrEmpty(certificate.FriendlyName))
                Label = $"{certificate.FriendlyName} ({Subject})";
            else
                Label = Subject;
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            EncryptionKey?.Dispose();
            EncryptionKey = null;
        }

        /// <inheritdoc/>
        public override string ToString()
            => Label;
    }
}
