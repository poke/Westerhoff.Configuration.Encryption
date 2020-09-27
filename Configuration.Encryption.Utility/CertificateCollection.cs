using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace Westerhoff.Configuration.Encryption.Utility
{
    /// <summary>
    /// Certificate collection.
    /// </summary>
    public class CertificateCollection : IDisposable
    {
        private CertificateInfo[] _certificates;

        /// <summary>
        /// Get list of certificates.
        /// </summary>
        /// <param name="reload">Whether to reload the certificates.</param>
        /// <returns>List of certificates.</returns>
        public CertificateInfo[] GetCertificates(bool reload = false)
        {
            if (reload)
                DisposeCertificates();

            if (_certificates is null)
                _certificates = LoadCertificatesFromStore();
            return _certificates;
        }

        /// <summary>
        /// Dispose all stored certificates.
        /// </summary>
        private void DisposeCertificates()
        {
            if (_certificates is null)
                return;

            foreach (var certificate in _certificates)
                certificate.Dispose();
            _certificates = null;
        }

        /// <inheritdoc/>
        public void Dispose()
            => DisposeCertificates();

        /// <summary>
        /// Load data encipherment certificates from the certificate store.
        /// </summary>
        /// <param name="storeName">Certificate store name.</param>
        /// <param name="storeLocation">Certificate store location.</param>
        /// <returns>Valid certificates.</returns>
        public static CertificateInfo[] LoadCertificatesFromStore(StoreName storeName = StoreName.My, StoreLocation storeLocation = StoreLocation.LocalMachine)
        {
            using var store = new X509Store(storeName, storeLocation);
            X509Certificate2Collection storeCertificates = null;
            try
            {
                store.Open(OpenFlags.ReadOnly);
                storeCertificates = store.Certificates;

                return storeCertificates
                    .Find(X509FindType.FindByKeyUsage, X509KeyUsageFlags.DataEncipherment, validOnly: false)
                    .OfType<X509Certificate2>()
                    .Select(c => new CertificateInfo(c))
                    .OrderBy(c => c.Label)
                    .ToArray();
            }
            finally
            {
                // dispose all certificates
                if (storeCertificates is object)
                {
                    foreach (var c in storeCertificates)
                    {
                        c.Dispose();
                    }
                }
            }
        }
    }
}
