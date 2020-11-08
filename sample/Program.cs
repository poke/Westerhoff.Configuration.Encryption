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
                    builder.Sources.Clear();
                    builder.AddEncryptedJsonFile("securesettings.json", "CN=ConfigurationEncryptionExample1", requireValidCertificates: false);
                });
    }
}
