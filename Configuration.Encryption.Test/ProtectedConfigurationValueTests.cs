using System.Collections.Generic;
using Xunit;

namespace Westerhoff.Configuration.Encryption.Test
{
    public class ProtectedConfigurationValueTests
    {
        [Theory]
        [MemberData(nameof(ConfigurationValues))]
        public void Parse(string configurationValue, string expectedSecret, string expectedThumbprint, string expectedLabel)
        {
            var result = ProtectedConfigurationValue.Parse(configurationValue);

            Assert.Equal(expectedSecret, result.EncryptedValue.ToString());
            Assert.Equal(expectedThumbprint, result.CertificateThumbprint.ToString());
            Assert.Equal(expectedLabel, result.Label.ToString());
        }

        [Fact]
        public void Parse_InvalidReturnsDefault()
        {
            var result = ProtectedConfigurationValue.Parse("Invalid value");

            Assert.True(result.EncryptedValue.IsEmpty);
            Assert.True(result.CertificateThumbprint.IsEmpty);
            Assert.True(result.Label.IsEmpty);
        }

        public static IEnumerable<object[]> ConfigurationValues()
        {
            // configuration values with only the protected secret
            yield return new[]
            {
                "§#§zO+36xLqeUcA9hgJ+lwtFmixWdDlBi6ziLIptp0DWvzWhASBIXXKaHPnFirA2GyJ7+C1qG1y75Ofx86UtcvyqAKvKpDquZ7wzG/2VC7wzMs8jZMcJm83FSx+j2t2358bDoZIOpUGrBN9yU9EiE93UnePUiopD4HzZJMsArQIiKqQhVaS4GjScxQozZUu2QuSfbB7mxsdjNK6PUuDOp5QmsUFaC8lzqIBiPRiUNTINuBP1a8fbBvglMyeTm5JH30p8xnbOyBUxNgmjnT8U5ZiGVGa6c3T5W65l1HbLmDBQ5Gk02rcNxSPE7M9e/JpQttFqtmznzyu04v2vm8fcJPLjUeEJoJgmiw0X4RoIeU7vQIsGWkwgWbzPZz5EB9p1N7kDjqTTa9RwkuBNOTMYVJfsyhd4mkkHoDWFErwYFfZ5YY4zKw+k1B3Q2FgI42/YbPbYmXeNb6//bGj4rMW48udAbOsuqim4sP74Xw+L4ldyUIUk/UMDSQnpzATVVBTwXEQlUvp4C6hjoPbE+tN7L7tThdM1KHxe1B7ubxleKRpCo4YvcGP5XMltLl+hwuHGWOq9OoEU2sRPP/rFh6COs1ZsYfJ/4j86XYoTT0v8zCuIHtdQwUMQr4jI4mlSV1e7sOtrL97+Gtqjgo0XCcUUgiwwQH4yHnNfveYVB9Td0no/+g=",
                "zO+36xLqeUcA9hgJ+lwtFmixWdDlBi6ziLIptp0DWvzWhASBIXXKaHPnFirA2GyJ7+C1qG1y75Ofx86UtcvyqAKvKpDquZ7wzG/2VC7wzMs8jZMcJm83FSx+j2t2358bDoZIOpUGrBN9yU9EiE93UnePUiopD4HzZJMsArQIiKqQhVaS4GjScxQozZUu2QuSfbB7mxsdjNK6PUuDOp5QmsUFaC8lzqIBiPRiUNTINuBP1a8fbBvglMyeTm5JH30p8xnbOyBUxNgmjnT8U5ZiGVGa6c3T5W65l1HbLmDBQ5Gk02rcNxSPE7M9e/JpQttFqtmznzyu04v2vm8fcJPLjUeEJoJgmiw0X4RoIeU7vQIsGWkwgWbzPZz5EB9p1N7kDjqTTa9RwkuBNOTMYVJfsyhd4mkkHoDWFErwYFfZ5YY4zKw+k1B3Q2FgI42/YbPbYmXeNb6//bGj4rMW48udAbOsuqim4sP74Xw+L4ldyUIUk/UMDSQnpzATVVBTwXEQlUvp4C6hjoPbE+tN7L7tThdM1KHxe1B7ubxleKRpCo4YvcGP5XMltLl+hwuHGWOq9OoEU2sRPP/rFh6COs1ZsYfJ/4j86XYoTT0v8zCuIHtdQwUMQr4jI4mlSV1e7sOtrL97+Gtqjgo0XCcUUgiwwQH4yHnNfveYVB9Td0no/+g=",
                "",
                "",
            };
            yield return new[]
            {
                "§#§YIcnzLqc2L/zzqM6qHIwFOlehT1SDYPtkyYuMavOHTu4B8tqDJnkguDhASDFIXSK9++FtsLu3kOG3HQX6A1TNkRNiCRZUT7TZBaL1cjHbnc+7DcsiCKd2lWjM+V9lSyCiyQeXqDc82j15LPpgWT/J7/EpKjciHl8SMZltf7j6+fuNohqjiaEJ723XfpTdg7MTUDti7Cjr1TAGwJ7iTxEZt1wV8Y6lzC6YeNsubBPCzBDsaaQrg8xKbKgyMt3fHPLMrN1bjVCuS5iT53Dq6JmzaV36AMpaIyEm+GTOqGn62mku+JdzgLWpaqrkh/ZvXl3h8tskaohR3G5FsoJKOwkySWt455l9T4iM7w7eOCnF+9cUV0CwRq2+F79qAI7sNxF6YQnYbZ31VSI3K5S37cEPg1sFLTmyr0nVPn9SbtBO5Ph7URl8TEg64Zu5LxGKHWTbsvwbB6rUvSNc7BfpyZJTRaypQ/WMTM13Kv4mokZvSWN8c3rv1XW4FWEvFCCXeF6GR/pLkscZSr+0zC+xIxsQt1jyDTsMREVpNZZuRcAh0vq68b2FeOqtSNrE/cLTa0KmmBThPCd4vYVpYm64BbsuKPBfZuggHAteimaka8hGJ7Sa5qhk4Uji4kJZsQuWMiJ6ELRxwTQ4N5uh4wy6H3PFuQtrdPjofbsyzpd4lFZPek=",
                "YIcnzLqc2L/zzqM6qHIwFOlehT1SDYPtkyYuMavOHTu4B8tqDJnkguDhASDFIXSK9++FtsLu3kOG3HQX6A1TNkRNiCRZUT7TZBaL1cjHbnc+7DcsiCKd2lWjM+V9lSyCiyQeXqDc82j15LPpgWT/J7/EpKjciHl8SMZltf7j6+fuNohqjiaEJ723XfpTdg7MTUDti7Cjr1TAGwJ7iTxEZt1wV8Y6lzC6YeNsubBPCzBDsaaQrg8xKbKgyMt3fHPLMrN1bjVCuS5iT53Dq6JmzaV36AMpaIyEm+GTOqGn62mku+JdzgLWpaqrkh/ZvXl3h8tskaohR3G5FsoJKOwkySWt455l9T4iM7w7eOCnF+9cUV0CwRq2+F79qAI7sNxF6YQnYbZ31VSI3K5S37cEPg1sFLTmyr0nVPn9SbtBO5Ph7URl8TEg64Zu5LxGKHWTbsvwbB6rUvSNc7BfpyZJTRaypQ/WMTM13Kv4mokZvSWN8c3rv1XW4FWEvFCCXeF6GR/pLkscZSr+0zC+xIxsQt1jyDTsMREVpNZZuRcAh0vq68b2FeOqtSNrE/cLTa0KmmBThPCd4vYVpYm64BbsuKPBfZuggHAteimaka8hGJ7Sa5qhk4Uji4kJZsQuWMiJ6ELRxwTQ4N5uh4wy6H3PFuQtrdPjofbsyzpd4lFZPek=",
                "",
                "",
            };

            // configuration values with the protected secret and a thumbprint
            yield return new[]
            {
                "§#§305AE2FB5D78E57688DF99E602CC1219622EB92F§l0nXsnh12NXXVMJ0KRkNMnF+pWKk/oXx68NAMpNEsRi59p14g/Y6rSrknc+nuqkrEtvqc1Bt9XQA0IhjVd17TzgnVgJ7NaUdMe+55/6dlH0BUSIuNFoDTpBHDU5+TGypeMOeTmfXD9c2ifrGObvzEb5v3RTtUSpp2Cl70RQqvTBuDsGpoKV3sFz4xwYzdk2y/MpXXIi3HEzBLmbDeTB6g+0qFI8U/UfVNsCtO0+XSfnTRqd5z0TsUyTTxJ5lZrb0WiKZ5O+9Kbtrr0p3dpYwLn1WpY1rhgecY1zjTyUNM53t1iyGQH3xV/qRSQi0uI9qHJU4SpSpSsaqNV9241lBggI2Kz90y64OMuurV8fYCdz8cG63QkmMLCAhSHoMkR/SD+rOC3Sa5hxL+yISyNCnU1Ys/nRtfPxqQZTdBkGo1imf8/Ss1ceJF9Hh8yVrFKQWoXsyiU5bqzh31YZ386OHZQQLRZxc0GlKL7awtxEOdOhVf4AbMIVBcQw0FPE1/01Qfd+7haKwnuzUxzWKdAheUTNIEa3MJJI4ZbGs3y6SrteIjlgULMDsW+wxt/ZOHa648m3dFXUWL371pXmrI1orJAfKfHWOE2sqyP+VEPUF9PiQN0/r86JciEcejOfBU2RgWKDTmU4CPXtA8z9gtcnXjO3ep4ddnqWTsNnCTGR6MDU=",
                "l0nXsnh12NXXVMJ0KRkNMnF+pWKk/oXx68NAMpNEsRi59p14g/Y6rSrknc+nuqkrEtvqc1Bt9XQA0IhjVd17TzgnVgJ7NaUdMe+55/6dlH0BUSIuNFoDTpBHDU5+TGypeMOeTmfXD9c2ifrGObvzEb5v3RTtUSpp2Cl70RQqvTBuDsGpoKV3sFz4xwYzdk2y/MpXXIi3HEzBLmbDeTB6g+0qFI8U/UfVNsCtO0+XSfnTRqd5z0TsUyTTxJ5lZrb0WiKZ5O+9Kbtrr0p3dpYwLn1WpY1rhgecY1zjTyUNM53t1iyGQH3xV/qRSQi0uI9qHJU4SpSpSsaqNV9241lBggI2Kz90y64OMuurV8fYCdz8cG63QkmMLCAhSHoMkR/SD+rOC3Sa5hxL+yISyNCnU1Ys/nRtfPxqQZTdBkGo1imf8/Ss1ceJF9Hh8yVrFKQWoXsyiU5bqzh31YZ386OHZQQLRZxc0GlKL7awtxEOdOhVf4AbMIVBcQw0FPE1/01Qfd+7haKwnuzUxzWKdAheUTNIEa3MJJI4ZbGs3y6SrteIjlgULMDsW+wxt/ZOHa648m3dFXUWL371pXmrI1orJAfKfHWOE2sqyP+VEPUF9PiQN0/r86JciEcejOfBU2RgWKDTmU4CPXtA8z9gtcnXjO3ep4ddnqWTsNnCTGR6MDU=",
                "305AE2FB5D78E57688DF99E602CC1219622EB92F",
                "",
            };
            yield return new[]
            {
                "§#§4CF559F7B0E11E9E6F2415419679D943418C0E13§avdK1MBKQCwmhjiMZP8lzBIC7Yg/kh/yQnexInKTDDq58l61mb94sSz9kQbyEtZQ8brJm3JdwLqZgwKaRql/iMXzPbbsa5PWl33CvOSZpFFBKkRzeXseMpW5NHafISdarEBU4NTo9Et4Z6iJB1W8dV0W247zEMy4wlg0CceRsqPUGZzhy1fZkErwOpbo4dEPUfuW9v7nd/1ys4BHRBqkULGkWw6nH61RUnyQOXIEOxxN7INHD5bo5vLx4owUOT4AI+ff56IZJDU4duqjQpNzc405hjeq6QO4TzO3eEdZyYjekKRr9FU4OtTqJ6NT0h578lXFBmL1f5HC4SMed9AUHies1LR3yQk1UkfGXQbf1i7KXQn/a61yPNaDd9rXtHhoywx5Z/GdXnItICCbI7dCf4NZHEca8yE0ervtd2U/uKabethKXF/IGPnvlsACpe3gorB82siH9Z6rer6YW0ooeQS5IE4xRCh49YsDZ67WogSkoS6ANv7v6YU3qcsENrSF5jWHnMAUev/gtH+w26Aq3kZpi/gt/cVLLv7vCSWw6r7Nn6DCWpqZ0BszR8LwXwP/+zrBRNH/1Uv3s72+5r6cd1pahcYZfAr6cZ4+/m5popBQxgYo0e2UhW9o2XOqOKQ++w5EPu+O560xFVGOzqYBaGxY2TGvNj/4OJg03loRWjU=",
                "avdK1MBKQCwmhjiMZP8lzBIC7Yg/kh/yQnexInKTDDq58l61mb94sSz9kQbyEtZQ8brJm3JdwLqZgwKaRql/iMXzPbbsa5PWl33CvOSZpFFBKkRzeXseMpW5NHafISdarEBU4NTo9Et4Z6iJB1W8dV0W247zEMy4wlg0CceRsqPUGZzhy1fZkErwOpbo4dEPUfuW9v7nd/1ys4BHRBqkULGkWw6nH61RUnyQOXIEOxxN7INHD5bo5vLx4owUOT4AI+ff56IZJDU4duqjQpNzc405hjeq6QO4TzO3eEdZyYjekKRr9FU4OtTqJ6NT0h578lXFBmL1f5HC4SMed9AUHies1LR3yQk1UkfGXQbf1i7KXQn/a61yPNaDd9rXtHhoywx5Z/GdXnItICCbI7dCf4NZHEca8yE0ervtd2U/uKabethKXF/IGPnvlsACpe3gorB82siH9Z6rer6YW0ooeQS5IE4xRCh49YsDZ67WogSkoS6ANv7v6YU3qcsENrSF5jWHnMAUev/gtH+w26Aq3kZpi/gt/cVLLv7vCSWw6r7Nn6DCWpqZ0BszR8LwXwP/+zrBRNH/1Uv3s72+5r6cd1pahcYZfAr6cZ4+/m5popBQxgYo0e2UhW9o2XOqOKQ++w5EPu+O560xFVGOzqYBaGxY2TGvNj/4OJg03loRWjU=",
                "4CF559F7B0E11E9E6F2415419679D943418C0E13",
                "",
            };

            // configuration values with the protected secret and a label
            yield return new[]
            {
                "§#§Optional label#M0Hu0D9CJhgt5stgH9UMH2N+zXfBCoJNexQO0wHGt3AkBciYaLXNWs4sM0DYB1xkjGjbOIWTpjDERjInjWDma/SVsDPR3LsRNPnQtNUzZ0RdkMNX7V4oNCPSrrVf0uVqxA1WRu6JzuRiIyNOwkLKwLzfzGLvW+SpwoZ2cqlbt0fnBMkkAgqVEWn3G8zudSEP/ZrJ1LaLWqaWAVbN3yRJsDjRwAWovED8xsMyaRlQVRrdMZP6DRtn0jLYD31C+zgGs8gaQCD7zwwLmEH5qWPZ1GqL7Y8XvUteOSPgLuBy7t16Wb2C8j9egbpCkxrGH9ThWWdVCuw423bEFcLqSgNVr5Cs+LxQmV/Z/56fJSxqZ3NSLrcYbLJL4ysuXA9p9Bm4P/JoXBIJ1SZSQEcZaA8xcwini9R48Ln8tJqSbVQn8GwEY3utfY+DQ4gbDZFPqBmo5IxSb1YbS7KM5aaZPHzsKWpOU89/a0b436o5Qhg9eohZWSEZEt/92rfDyGk/aJ2tOpUJLZaT3iqD0yR1GPWNsNqGNMjaIzbSvAUpZn8Pa/rzN53EJjgT7nYeGPvYJPyDsAR9FJTo09ifGLf2KPU5r+cFI1qH7ibdUAjfcbDTMc+JZAeu+hQdguigdIZLvnK5OlWLRoFinKZLjoXJa1zC+sPqBsPnYj91pfEcTRKiILc=",
                "M0Hu0D9CJhgt5stgH9UMH2N+zXfBCoJNexQO0wHGt3AkBciYaLXNWs4sM0DYB1xkjGjbOIWTpjDERjInjWDma/SVsDPR3LsRNPnQtNUzZ0RdkMNX7V4oNCPSrrVf0uVqxA1WRu6JzuRiIyNOwkLKwLzfzGLvW+SpwoZ2cqlbt0fnBMkkAgqVEWn3G8zudSEP/ZrJ1LaLWqaWAVbN3yRJsDjRwAWovED8xsMyaRlQVRrdMZP6DRtn0jLYD31C+zgGs8gaQCD7zwwLmEH5qWPZ1GqL7Y8XvUteOSPgLuBy7t16Wb2C8j9egbpCkxrGH9ThWWdVCuw423bEFcLqSgNVr5Cs+LxQmV/Z/56fJSxqZ3NSLrcYbLJL4ysuXA9p9Bm4P/JoXBIJ1SZSQEcZaA8xcwini9R48Ln8tJqSbVQn8GwEY3utfY+DQ4gbDZFPqBmo5IxSb1YbS7KM5aaZPHzsKWpOU89/a0b436o5Qhg9eohZWSEZEt/92rfDyGk/aJ2tOpUJLZaT3iqD0yR1GPWNsNqGNMjaIzbSvAUpZn8Pa/rzN53EJjgT7nYeGPvYJPyDsAR9FJTo09ifGLf2KPU5r+cFI1qH7ibdUAjfcbDTMc+JZAeu+hQdguigdIZLvnK5OlWLRoFinKZLjoXJa1zC+sPqBsPnYj91pfEcTRKiILc=",
                "",
                "Optional label",
            };
            yield return new[]
            {
                "§#§Custom label in the configuration value#OasS+tTubwa/+6KFbFNJItHyZ0pUivYVguAjCZpQEqeWx1KabaxqJSCkCZ+2kwIvAVZ2reictOAyqK/IqRn33UqWENPSaAcKnVEGYGHJjBoFkYinLYcSqsyO5VAq9VL59sXxMGsOrB6LS0WzaF0LQBum0BVBosDt3RUYyZkhUsyEayrPMPmjvzH4D+FrRc8oxKvuBIw+U3JeFnw/FgpQLkktyhhJRFWXIYb3z+XcxSsnZj/J0PzV67e9EOSyXxlmI1ErmyyoD3kPknZ9FwItm7aIqWji/cFdc9M1z4IWbnzOnOuyneWh4q97JdjPzPWiMo4as89J9Pr0ZHi1oXSUcGvnLl50E65Qb1IBzTti437DH3DiqzapvLJl3NSmITsugOuprXaUbHR732lRIaK5ebnQdImuEll67qhcllcuIbfxRYehsmfDBFdEkYA7GtOpn5GEvSfCzWRqdyrg4WrdpcOM7T+r0XW5Jy4Y9zNXtlbXflSUp8OoRKv8dOQ56oOTwKJ0F7ZwFdTVdVsw6xVAAv0q3Q1g84o9gz3C0Pww/HI2VSpJI+3bCd6puTlGOHgGyy5+nuBsIUWfUVnlEvSPdzc7humz0y/1XEYGaJj4mRz5ymt0J0KD1uq4qs0lHqMOK61Y3EJ1dGm/WKfkrQDIOwLpDv44P/lJcKzK8+q9+FE=",
                "OasS+tTubwa/+6KFbFNJItHyZ0pUivYVguAjCZpQEqeWx1KabaxqJSCkCZ+2kwIvAVZ2reictOAyqK/IqRn33UqWENPSaAcKnVEGYGHJjBoFkYinLYcSqsyO5VAq9VL59sXxMGsOrB6LS0WzaF0LQBum0BVBosDt3RUYyZkhUsyEayrPMPmjvzH4D+FrRc8oxKvuBIw+U3JeFnw/FgpQLkktyhhJRFWXIYb3z+XcxSsnZj/J0PzV67e9EOSyXxlmI1ErmyyoD3kPknZ9FwItm7aIqWji/cFdc9M1z4IWbnzOnOuyneWh4q97JdjPzPWiMo4as89J9Pr0ZHi1oXSUcGvnLl50E65Qb1IBzTti437DH3DiqzapvLJl3NSmITsugOuprXaUbHR732lRIaK5ebnQdImuEll67qhcllcuIbfxRYehsmfDBFdEkYA7GtOpn5GEvSfCzWRqdyrg4WrdpcOM7T+r0XW5Jy4Y9zNXtlbXflSUp8OoRKv8dOQ56oOTwKJ0F7ZwFdTVdVsw6xVAAv0q3Q1g84o9gz3C0Pww/HI2VSpJI+3bCd6puTlGOHgGyy5+nuBsIUWfUVnlEvSPdzc7humz0y/1XEYGaJj4mRz5ymt0J0KD1uq4qs0lHqMOK61Y3EJ1dGm/WKfkrQDIOwLpDv44P/lJcKzK8+q9+FE=",
                "",
                "Custom label in the configuration value",
            };

            // configuration values with the protected secret, a thumprint, and a label
            yield return new[]
            {
                "§#§Optional config label#305AE2FB5D78E57688DF99E602CC1219622EB92F§McuNqwgdVf2Z2gB5X7xVcvT2SzfA+NwmaiiUDYXUsCB783D0UdZtnZRFCdyFqfCoP5xeAxnevDc/zSW8pcCHpd/jHcw/NVRainbCNidJ3OTIPBLFhKPoIBeJoKYmp3cA8AFxq82oz5TEFHljw611YuHDvdkMxOQzm1o9IyOuIv7WRsJVPQHHu+7KQ39wXevOe63lKrl073vXLdT24X9CcTca93AraDU0CC2yHwK70OKmjgpqqPMt+0Ou7G48Cd0IZYLIF5keBM+lb7H/88W342jXPhQ5AhwVAXDaqAWG4f8JuE7bzqBNABfIgC8UN0b0nDRmUee3Ml1+m1Xx+5N4u67iZeJOhIz+wefSYT2uP2IVM75vwvsajgIFU78SeTSlyeeELlsXcFnCaspNOsftZXpQ2ZbD3lPoLb1w0F2/xzZ7IDUsbvfTlvbuZXNxsTkq65c6rX5bsgmZ9GXv+xvCtjhGa22omgIttL3aPNG5zSVYRdN4qH4ZIkv+mkz47xuNDDWoakpGZMFGWIrrRtcmEALFIo0yKIgypt5rMHYftBbrPc0pvZ3Ub35C78zAni+yTFc97aM5BOaGLsW3fxeHP1xwYaS0+7Ga903XZjsIwKLIgmDPK0IfZyPAafycS6Uio82LSksOMuGzXMo894OuiLBPvaCiNTq02G6HiXnGj2I=",
                "McuNqwgdVf2Z2gB5X7xVcvT2SzfA+NwmaiiUDYXUsCB783D0UdZtnZRFCdyFqfCoP5xeAxnevDc/zSW8pcCHpd/jHcw/NVRainbCNidJ3OTIPBLFhKPoIBeJoKYmp3cA8AFxq82oz5TEFHljw611YuHDvdkMxOQzm1o9IyOuIv7WRsJVPQHHu+7KQ39wXevOe63lKrl073vXLdT24X9CcTca93AraDU0CC2yHwK70OKmjgpqqPMt+0Ou7G48Cd0IZYLIF5keBM+lb7H/88W342jXPhQ5AhwVAXDaqAWG4f8JuE7bzqBNABfIgC8UN0b0nDRmUee3Ml1+m1Xx+5N4u67iZeJOhIz+wefSYT2uP2IVM75vwvsajgIFU78SeTSlyeeELlsXcFnCaspNOsftZXpQ2ZbD3lPoLb1w0F2/xzZ7IDUsbvfTlvbuZXNxsTkq65c6rX5bsgmZ9GXv+xvCtjhGa22omgIttL3aPNG5zSVYRdN4qH4ZIkv+mkz47xuNDDWoakpGZMFGWIrrRtcmEALFIo0yKIgypt5rMHYftBbrPc0pvZ3Ub35C78zAni+yTFc97aM5BOaGLsW3fxeHP1xwYaS0+7Ga903XZjsIwKLIgmDPK0IfZyPAafycS6Uio82LSksOMuGzXMo894OuiLBPvaCiNTq02G6HiXnGj2I=",
                "305AE2FB5D78E57688DF99E602CC1219622EB92F",
                "Optional config label",
            };
            yield return new[]
            {
                "§#§Custom label to add something readable#4CF559F7B0E11E9E6F2415419679D943418C0E13§qPV3E8to+ZH6Ko3R2pOHzpKV308jh09MeydPrTnXoUsHooAOtO8gQ+Y0r30M7v+b9yqv6PhvcGCCObwuwFnIr82WCzkvbQfn9Fj5Vwpfyxm+sQoIYzoTMiez+tP917rQ9L31Ca01Rb0uAA4uUamkj1Xg5VPS1w1XdySXXlHOmeum3tC3BfRA6gBSnM9hqZN/dsj30E0KyGCflxsDw7BConb+9CIWSXSGIJ6ArmIkLvnUIfWB5SZhKayDxHlUbb2eaMRQMFppvhjE3fJ/ZCbFMl76YQOfOFL2ttTspRWDd4D2kwTMpPcA//N7Rh94pKfR6/KRqqict0yE5Y9vw66LPezUwmoba3sfXkm21BOoq0s+edHtLKdiFXrJz+SkBVIPMLGopfgQmCwi8QownFRz0A193f/mX4YSTqNwdH4zwrVA06AI5csck/85el3qfKA0EFTjwjZOEFozg0ABMhzF6OtyDjEB0WsTwa3CgxuHqKd66EO+VsC9yhufTpfqG/81C0o8kAGEYycyLVCGeBAjaCC9IzcSQXa4jIKW3RcK6r3ZuHHqWWwaif7c+7X1pJliMG3GO8JKHKMCq5cHq0J4GApFpmnQhLKIiJjUv57qGjiIJa4cBQePLGMWJi1ECIYJ/hLdZ1k/oZhjxYvQOgrEnRqwiCVOMSTqFyrdO7d7J0c=",
                "qPV3E8to+ZH6Ko3R2pOHzpKV308jh09MeydPrTnXoUsHooAOtO8gQ+Y0r30M7v+b9yqv6PhvcGCCObwuwFnIr82WCzkvbQfn9Fj5Vwpfyxm+sQoIYzoTMiez+tP917rQ9L31Ca01Rb0uAA4uUamkj1Xg5VPS1w1XdySXXlHOmeum3tC3BfRA6gBSnM9hqZN/dsj30E0KyGCflxsDw7BConb+9CIWSXSGIJ6ArmIkLvnUIfWB5SZhKayDxHlUbb2eaMRQMFppvhjE3fJ/ZCbFMl76YQOfOFL2ttTspRWDd4D2kwTMpPcA//N7Rh94pKfR6/KRqqict0yE5Y9vw66LPezUwmoba3sfXkm21BOoq0s+edHtLKdiFXrJz+SkBVIPMLGopfgQmCwi8QownFRz0A193f/mX4YSTqNwdH4zwrVA06AI5csck/85el3qfKA0EFTjwjZOEFozg0ABMhzF6OtyDjEB0WsTwa3CgxuHqKd66EO+VsC9yhufTpfqG/81C0o8kAGEYycyLVCGeBAjaCC9IzcSQXa4jIKW3RcK6r3ZuHHqWWwaif7c+7X1pJliMG3GO8JKHKMCq5cHq0J4GApFpmnQhLKIiJjUv57qGjiIJa4cBQePLGMWJi1ECIYJ/hLdZ1k/oZhjxYvQOgrEnRqwiCVOMSTqFyrdO7d7J0c=",
                "4CF559F7B0E11E9E6F2415419679D943418C0E13",
                "Custom label to add something readable",
            };
        }
    }
}
