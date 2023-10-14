using Microsoft.Extensions.Configuration;
using MySqlConnector;
using NPoco;
using StackExchange.Profiling.Internal;
using System;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace Core
{
    public class Utility
    {
        public static IConfiguration Config => LoadConfig();
        public static IConfigurationRoot LoadConfig()
        {
            return new ConfigurationBuilder()
                .SetBasePath(System.AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json").Build();
        }

        public Utility()
        {
            LoadConfig();
        
        }
      
        public static string GetHash(string text)
        {
            var encodedPassword = new UTF8Encoding().GetBytes(text);
            var hash = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(encodedPassword);
            var encoded = BitConverter.ToString(hash)
               .Replace("-", string.Empty)
               .ToLower();
            return encoded;
        }
        public static byte[] GetURLToBytes(string filePath)
        {
            byte[] byteArray = new byte[0];
            try
            {
                var webClient = new WebClient();
                byteArray = webClient.DownloadData(filePath);
                webClient.Dispose();
                return byteArray;
            }
            catch
            {
                //
            }
            return byteArray;
        }

        public static bool CacheEnabled => true;
        public class HttpException : Exception
        {
            private readonly int httpStatusCode;

            public HttpException(int httpStatusCode, string message) : base(message)
            {
                this.httpStatusCode = httpStatusCode;
            }
        }
        public static string GetBaseUrl()
        {
            if (MyHttpContext.Current == null || MyHttpContext.Current.Request == null) return "";
            var request = MyHttpContext.Current?.Request;
            var host = request?.Host.ToUriComponent();
            var pathBase = request?.PathBase.ToUriComponent();
            var url = $"{request?.Scheme}://{host}{pathBase}" + "/";
            return url.Replace("http", "https");
        }

        private static string ConnectionString =>
            Config.GetSection("ConnectionString").GetSection("Default").Value;

        public static IDatabase Database =>
            new ProfiledDatabase(ConnectionString, DatabaseType.MySQL, MySqlConnectorFactory.Instance);  //   MySqlClientFactory.Instance);  use old database versions

        public class AppConfig
        {
            public string GetValue(string section, string defaultValue)
            {
                var sec = Config.GetSection(section);
                if (sec == null) return defaultValue;
                return sec.Value == null ? defaultValue : sec.Value;
            }

            public string GetValue(string section, string subSection, string defaultValue)
            {
                var sec = Config.GetSection(section);
                if (sec == null) return defaultValue;
                var subSec = sec.GetSection(subSection);
                if (subSec == null) return defaultValue;
                return subSec.Value == null ? defaultValue : subSec.Value;
            }
        }

        public static bool IsValidateMobileNumber(string number)
        {
            if (number.IsNullOrWhiteSpace()) return false;
            return number.Length >= 10;
        }
        public static int GetAge(DateTime dob)
        {
            var today = DateTime.Today;
            var age = today.Year - dob.Year;
            if (dob.Date > today.AddYears(-age)) 
                age--;
            return age;
        }

        public static string GetYearFromAge(int age)
        {
            var today = DateTime.Today;
            var year = today.Year - age;
            return year.ToString();
        }

        
        
        public static string CheckLanguage()
        {
            string lang = "en";
            try
            {
                var headers = MyHttpContext.Current.Request.Headers;
                lang = headers["Language"];
                if (lang.IsNullOrWhiteSpace())
                    lang = "en";
            }
            catch 
            {
                //
            }
            return lang;
        }
      
    }
}
