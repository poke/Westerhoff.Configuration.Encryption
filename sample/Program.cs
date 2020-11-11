using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EncryptionSample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            var configuration = host.Services.GetService<IConfiguration>();

            foreach (var config in configuration.AsEnumerable())
            {
                Console.WriteLine($"{config.Key} = {config.Value}");
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(builder =>
                {
                    // augment existing JSON file sources (appsettings.json, appsettings.<env>.json)
                    builder.EncryptJsonFileSources("CN=ConfigurationEncryptionExample2", requireValidCertificates: false);

                    // add an additional encrypted JSON file source
                    builder.AddEncryptedJsonFile("securesettings.json", "CN=ConfigurationEncryptionExample1", requireValidCertificates: false);
                });
    }
}
