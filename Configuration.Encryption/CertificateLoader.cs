using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace Westerhoff.Configuration.Encryption
{
    /// <summary>
    /// Certificate loader.
    /// </summary>
    public static class CertificateLoader
    {
        /// <summary>
        /// Load a certificate from the certificate store.
        /// </summary>
        /// <param name="distinguishedName">Distinguished name of the certificate.</param>
        /// <param name="storeName">Certificate store name.</param>
        /// <param name="storeLocation">Certificate store location.</param>
        /// <param name="validOnly">Whether to only look for a valid certificate.</param>
        /// <returns>Found certificate.</returns>
        public static X509Certificate2 LoadCertificateFromStore(string distinguishedName, StoreName storeName = StoreName.My, StoreLocation storeLocation = StoreLocation.LocalMachine, bool validOnly = true)
        {
            using (var store = new X509Store(storeName, storeLocation))
                return FindCertificate(store, X509FindType.FindBySubjectDistinguishedName, distinguishedName, validOnly);
        }

        /// <summary>
        /// Find a certificate in the certificate store.
        /// </summary>
        /// <param name="store">Certificate store.</param>
        /// <param name="findType">Certificate find type.</param>
        /// <param name="findValue">Search criteria.</param>
        /// <param name="validOnly">Whether to only look for a valid certificate.</param>
        /// <returns>Found certificate, or <c>null</c>.</returns>
        private static X509Certificate2 FindCertificate(X509Store store, X509FindType findType, object findValue, bool validOnly)
        {
            X509Certificate2Collection storeCertificates = null;
            X509Certificate2 certificate = null;
            try
            {
                store.Open(OpenFlags.ReadOnly);
                storeCertificates = store.Certificates;
                certificate = storeCertificates.Find(findType, findValue, validOnly)
                    .OfType<X509Certificate2>()
                    .OrderByDescending(c => c.NotAfter)
                    .FirstOrDefault();
                return certificate;
            }
            finally
            {
                // dispose all certificates except the found one
                if (storeCertificates is object)
                {
                    foreach (var c in storeCertificates)
                    {
                        if (c != certificate)
                            c.Dispose();
                    }
                }
            }
        }
    }
}
