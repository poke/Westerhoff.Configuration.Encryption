# Westerhoff.Configuration.Encryption

[![See this package on NuGet](https://img.shields.io/nuget/v/Westerhoff.Configuration.Encryption.svg?style=flat-square)](https://www.nuget.org/packages/Westerhoff.Configuration.Encryption)

A configuration provider for [`Microsoft.Extension.Configuration`](https://www.nuget.org/packages/Microsoft.Extensions.Configuration) that supports certificate-encrypted configuration values in a JSON file. The provider directly extends the default JSON provider from [`Microsoft.Extensions.Configuration.Json`](https://www.nuget.org/packages/Microsoft.Extensions.Configuration.Json), which makes any `appsettings.json` file automatically valid for this provider. When the JSON file has configuration values that match the encrypted configuration format described below, they will be decrypted at runtime, allowing you to specify secrets within your configuration files without them being exposed in plain text.

## Encrypted configuration value format

An encrypted configuration value is prefixed by the marker text `§#§` which signalizes the provider to attempt to decrypt the value. Encrypted values consist of up to four parts:

1. Constant marker prefix `§#§`
2. Optional label terminated by `#` that explains what the configuration value is used for in case you need to pass the encrypted value around.
3. Optional certificate thumbprint terminated by `§` which explicitly specifies which certificate to use to decrypt the value. If no certificate thumbprint is specified, the configured default certificate is used instead.
4. Encrypted value.

This means that the encrypted configuration value can take any of the following forms:

```json
{
  "DefaultCert_NoLabel": "§#§<encrypted-value>",
  "DefaultCert_WithLabel": "§#§<label>#<encrypted-value>",
  "SpecificCert_NoLabel": "§#§<thumbprint>§<encrypted-value>",
  "SpecificCert_WithLabel": "§#§<label>#<thumbprint>§<encrypted-value>",
}
```

For actual examples, using real encrypted values and thumbprints, check out the configuration files in the sample project.

## Using the library

When using the host builder, you can register the configuration source using the `AddEncryptedJsonFile` extension method on the configuration builder:

```csharp
Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration(builder =>
    {
        builder.AddEncryptedJsonFile("securesettings.json", "CN=ConfigurationEncryptionExample1", requireValidCertificates: false);
    });
```

### Creating a certificate

The library supports any *data encipherment* certificate with a sufficiently strong key. While reusing an existing certificate is theoretically possible, using a dedicated certificate created explicitly for the purpose of encrypting your configuration is recommended.

If you have a PKI set up, you can generate a properly signed certificate to use with the library. Otherwise, you can also create a self-signed certificate. This should be fine for production purposes too since the certificate is only used to protect data local to the machines it is installed on. Certificates used for decryption have to be installed into the personal certificate store of the local machine in order for the library to find them at runtime.

To create a self-signed certificate on Windows, you can use the following PowerShell command:

```powershell
New-SelfSignedCertificate -Subject "ConfigurationEncryption" -CertStoreLocation Cert:\LocalMachine\My -KeyLength 4096 -Type DocumentEncryptionCert
```

This will create a new certificate with the distinguished name `CN=ConfigurationEncryption` and import it into the personal certificate store.

After importing the certificates in the personal store, make sure to grant the account executing the application *read access* to the private key. You can do this in the certificate manager by selecting the _“Manage Private Keys”_ task from the certificate’s context menu.

## Protecting configuration values

To protect secret values using the certificate, you can use the `ConfigurationEncryptionUtility` program to generate the values using the locally installed certificates. If you need to protect secrets on a different machine, you can export the certificate *without the private key* and import it on the machine you want to protect the values on.
