using System;
using System.Security.Cryptography;
using Xunit;

namespace Westerhoff.Configuration.Encryption.Test
{
    public class EncryptionUtilityTest
    {
        private static readonly byte[] PrivateKey = Convert.FromBase64String("MIIEpAIBAAKCAQEAvg1EdLEXmxB0HIotc6UuLPycyNS+cRAwVrzUG+pF29HD6zoNO7YJ38UZpdf+HR5YnCU8HDSA5DUdSXKBpR5vP9GZ2XDrzGQwhnivs+CBNfLzoNKSMOBuuEceWijuP/kCBNDVzzOsTRPOsYrm2RDhqGa770Eg5NS/H4NOBuYT6q7UPLj1Q0L1FBurKte4mbdfnvVf0rO1wbcSWts5Uw6XL5HD1hPqIjOa1voog0KZ9yz18bVoXyrVC4MU0OI3jBw60Lyq8tmvkoGUvwVlsis/sjtZkhWHZcx/q07uLCxpqx88F5T0ZZ9muHG3qc3PwU5Gh5z2HjYE6E4BoFbVQk8zRQIDAQABAoIBAQC1bLDmvI+ORXyyKe4NsaeM5nE8/mn2QMAEbSoGo/OgTnS6vqYXVEXEycEcIj7AyVFJbfod6rkIIPQeQyMj3xPQVmSJwFQ4v4PbOqZwDmcKeyy7X4iTBAxt+eYtp1kY1LpqonkY3Xkj8z6jGbgZixX/zmKlmQSvra/tg0kfKM/mfOvKSc6OtfoCkPlmayiHZuKWV9RlyDD64Y1z9USQYD0I3PLHJl0bTxJD+SGekKunXQswLTkSty42Ipw6C1jHmRqyKWROw/4D2/6pTTI3JMeXg8rJDaAHzS93505bIdYAsoi3bSzlAdHeRfAzFekKvGqi5zowuC6ipOzg49Vqs4fZAoGBAPH5foIWjskUQISNgCTSp9RWiu0IyzStJjC1+vH0F9DFHwWbNui6o0/ALYLRi05q37n5UsQ2q0AYHZSF5BJl4z9XfbKvUmPLviEatxGDV+4DwpSmCm6jFUYXYvGbIyTTqO2NTS1sXQD/drjIV8EfGJYNo7YqauAG3C+Mdhgn2xGjAoGBAMkRUxzXOqmvbdSn0K/H0OeZp87gFYl4/PkHA6W7iWh38YmVJsHwnDX2Ue59tuSo/+1TizyCdF26jlqFir+/RROvPiyMyhTa4SHtu3qvyor99t2AwliD8Lxs6g3bd8NWdRkVTOg/o0hiCzsC/XV+g4uR52NbI1gqZ9zfeTxikwX3AoGBAMldQKPtNcGr8szcHEmIbFXN9yquaLxbYy6iIkT7Rl5bzCcUNauStZkGalk1G+KUGb5f9zqF7BkYeHVCvrt4wpXRry7lq4CU6pIN807FqITP6Dtfucq8OFXt3E8HNUXORgP51MH6LaNI9Hltb2mtAP7ONvC/d9UaoF2JXDVZ5XpVAoGAECi/SDsX8Nkzd3rBL89cP7C+psallHXPxufEaEZpJac2FzuQcNe4hjG0pfkegMQARX+IWiWV2o6KsEW5P+3MTGeyFHyAJSruGv7zKUoFeiQs+eJGPDzmVQ5fxRtVRbKmGIz4sFWk9sVCZ8y2uAGh6s6gDQEfFw/ZZwviv3KgpEMCgYAIpBSGMymr0MPk0oWwxlvMxPVAQfocsVVDPFBTF8hk44uPC06JLfDX23njdvlayH0yrfvSBAX686tCIeDgjpPLABsEhtqrJEd+WSn7kJNjWkKNplvOtTYGM3KV6+8xeWwXrjgQrVicA9uvptazQD8kDfWHqo39zUvgjiHQbHTdLA==");
        private static readonly byte[] PublicKey = Convert.FromBase64String("MIIBCgKCAQEAvg1EdLEXmxB0HIotc6UuLPycyNS+cRAwVrzUG+pF29HD6zoNO7YJ38UZpdf+HR5YnCU8HDSA5DUdSXKBpR5vP9GZ2XDrzGQwhnivs+CBNfLzoNKSMOBuuEceWijuP/kCBNDVzzOsTRPOsYrm2RDhqGa770Eg5NS/H4NOBuYT6q7UPLj1Q0L1FBurKte4mbdfnvVf0rO1wbcSWts5Uw6XL5HD1hPqIjOa1voog0KZ9yz18bVoXyrVC4MU0OI3jBw60Lyq8tmvkoGUvwVlsis/sjtZkhWHZcx/q07uLCxpqx88F5T0ZZ9muHG3qc3PwU5Gh5z2HjYE6E4BoFbVQk8zRQIDAQAB");

        [Theory]
        [InlineData("Foo bar", "FglWoiGrTzBXE2wbF1iV066uc96iQyPJS3xfdczLw0NpLA87cKwCuyoc4lqgfizK4rHQqFn3SzPO66K8jHFORz5cuXBP/F8YKDRbJE3RGoJZPVzvitsnYvE7d/O3+0tENq34QbVzv/wBHLa5eKU7YbUj8qLjDZzeBIrkx1lSthqvZx55GahyLpxMHdjZyLYO+9VjZMobSrUolwU6ntz/Fj6lQNqMBQ5W/EdmWPGHz5jpV1/7I1yA0nV8UkH/r+XmN2WP7DcF9SkEtuXm+gXR1V+ycqyqkuo4SBmLUKLgnN1S2dyr+KTY6TVfhYaA26kiGi6ORIniSBf7878L3VwB5w==")]
        [InlineData("Server=myServerAddress;Database=myDataBase;User Id=myUsername;Password=myPassword", "nl+8Qq70I7Oe8S8Eb3c7HWGSL0eWyGr/x3tolFGRFkIjgEvTilXqfVDXFcsCs8NjvTuKYYXCbapNI3tHFsx8R8xBfZZFCrvkXqFMOW6YPa77gutS2nKzE4AMAsMxDWhEC0oMlRGelHe+pdSd9Coln47ow1GvSGu9HetvLgayUuzfiCvg/Mc6XXBzUc/0Y2kKZLbJqg4+K6ktFIxzPUBGAnQw0Svf47uVpGvB29DhlysWd6eUX50TtX9dKIDcfRdn16gZ4KDU97hdIyQGcyytmmoPIK69gF3ZelWnrek5Xy3tLNOb58vvo/5QU3/v9scO1RLSlAlFDUhhRj8kaVAHmg==")]
        [InlineData("local\\useracc", "ZM7ILsjOXrsRZMnP1av+JEWsttc+ec7PSY63kwEvdT0XVCp9M/yLV6hBOtaOoRkM5zBJwgAIyAEUDbn0EBvnOrMeaCoGDiBBiNxeUJBwA5S9AliCSA9Uet9FT3m7mtyi8qukw4AvGFbd82+++3wk+Oa3xYrAFimYot7YM2rlRmr9eLayTmbfvblslh0i7wmBLOjf38l0KsA9x9LaLFUde9bM4b9bx1lTkkj7VJTqHxi0Xx39QzyH/q0+bfOQo0CBdTW/xcoDQR20wdla0HqTQIBWzN9xlwLVZ6IvjgbPNDjfrmIoXWXEaT1yz5R0LjzFSo3okB7Y8/zVFfgX5r7nAg==")]
        [InlineData("AVerySecurePassword", "m6klGqeUKFlMysLDYKyKbejYu1dAu5+xz3Q7LbdrXK6CwQCYmz2R55vAtyPLz0KiAlrJZndGgQ3mTYL3RNpXgZs3c7L4XkmGITNuJNUGlGRSipbvgM1QqCQrxlCyqQS5/qExKmXI78inpyOFEWdMu3ZDGm+1oZ+hllaK8gijA0iTZyOlfh0yC/oN/DQ5t4EoylDjQUzHm+whskas3u57wxQ9aPDouhm5dS26NMwuuAOqoS+XKDCDh3rJ6N35P3qMKKr2ASIqSjVHqaIbLb71zgnYumhJCFU3+jYS38UE3t4+h6uGaCiPNNLk9NMm6+UC0eYGDVyRFte4vraXAXpH5Q==")]
        [InlineData("rabbitmq://user:password@cluster.host/vhost", "sZ1rsUyOhwC0bvmnWt0o3xH/rV5RMMwCbiqOvy1ZIyo0y11XNmtiuVWaN5HHNvfbnW+WK+wdD8EJDeqvnsCK949bx9KrM7TZUCsRW/+RZk1gVeHBYmbnUzERLevbE5FJD6sRnnOUstyQR3qtRN5Vk0FzUks3XaVya5ahVDcys6aBZk0XtvtboWlQomvZjKK19WMdX5uXhD3DUSvwAitubIzhqBVdVZemxhN2DhEVj/Hf+5iqCF74HlCShU0rZ9sxSykTzhHKgyf/V1IRpM7aY2u7GRN0Mxw0LCHLSnGUlpaeWlG6hqVJX5jsA+N2a5Yc1Cfh/uXjPCpl3ijX/0ZJWg==")]
        public void Decrypt(string value, string encryptedValue)
        {
            var privateKey = RSA.Create();
            privateKey.ImportRSAPrivateKey(PrivateKey, out _);

            var result = EncryptionUtility.Decrypt(encryptedValue, privateKey);
            Assert.Equal(value, result);
        }

        [Theory]
        [InlineData("Foo bar")]
        [InlineData("Server=myServerAddress;Database=myDataBase;User Id=myUsername;Password=myPassword")]
        [InlineData("local\\useracc")]
        [InlineData("AVerySecurePassword")]
        [InlineData("rabbitmq://user:password@cluster.host/vhost")]
        public void EncryptRoundTrip(string value)
        {
            var privateKey = RSA.Create();
            privateKey.ImportRSAPrivateKey(PrivateKey, out _);
            var publicKey = RSA.Create();
            publicKey.ImportRSAPublicKey(PublicKey, out _);

            var encrypted = EncryptionUtility.Encrypt(value, publicKey);
            var decrypted = EncryptionUtility.Decrypt(encrypted, privateKey);
            Assert.Equal(value, decrypted);
        }
    }
}
