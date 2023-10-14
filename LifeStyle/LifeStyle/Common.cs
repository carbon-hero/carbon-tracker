using Core.Models;
using Newtonsoft.Json;
using NLog;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using Core.Repositories;

namespace LifeStyle
{
    public class Common
    {
        private static IConfiguration _config;
        public Common(IConfiguration config)
        {
            _config = config;
        }

        public static class Instances
        {
            public static DataTypeRepo DataTypeInst => new DataTypeRepo();
            public static DataTypeValuesRepo DataTypeValuesInst => new DataTypeValuesRepo();
            public static SnippetRepo SnippetInst => new SnippetRepo();
            public static AppSettingRepo AppSettingInst => new AppSettingRepo();
            public static FileRepo FileInst => new FileRepo();
            public static TemplateRepo TemplateInst => new TemplateRepo();
            public static UserRepo UserInst => new UserRepo();
            public static FriendRepo FriendInst => new FriendRepo();
            public static CommuteHistoryRepo CommuteHistoryInst => new CommuteHistoryRepo();
            public static UserPointTransactionRepo UserPointTransactionInst => new UserPointTransactionRepo();
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
        public static bool ValidateDateTimeString(string datetime)
        {
            return
                DateTime.TryParseExact(datetime, "yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out _);
        }

        public static bool ValidateDateString(string date)
        {
            return
                DateTime.TryParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out _);
        }

        public static bool ValidateDateStringForOldOrders(string date)
        {
            var validDate =
                DateTime.TryParseExact(date, "MM/dd/yyyy", CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out _);
            if (!validDate)
            {
                validDate = DateTime.TryParseExact(date, "M/dd/yyyy", CultureInfo.InvariantCulture,
                   DateTimeStyles.None, out _);
            }
            if (!validDate)
            {
                validDate = DateTime.TryParseExact(date, "M/dd/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture,
                   DateTimeStyles.None, out _);
            }
            if (!validDate)
            {
                validDate = DateTime.TryParseExact(date, "MM/dd/yyyy hh:mm:ss", CultureInfo.InvariantCulture,
                   DateTimeStyles.None, out _);
            }
            return validDate;
        }
        public static string CreatePassword(int length)
        {
            const string valid = "1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }

        public static void FixNull<T>(ref T obj)
        {
            if (obj == null) return;
            foreach (var propertyInfo in obj.GetType().GetProperties())
            {
                try
                {
                    if (propertyInfo.PropertyType == typeof(string))
                    {
                        if (propertyInfo.GetValue(obj, null) == null)
                        {
                            propertyInfo.SetValue(obj, string.Empty, null);
                        }
                    }
                }
                catch (Exception ex)
                {
                    //do nothing for now
                }
            }
        }

        public static string UploadPath => $"AppData/Upload/{DateTime.Now.Year}/{DateTime.Now.Month}";

        public static int GenerateRandomNo()
        {
            int _min = 1000;
            int _max = 9999;
            Random _rdm = new Random();
            return _rdm.Next(_min, _max);
        }

        public class Log
        {
            private static readonly Logger _logger = new LogFactory().GetCurrentClassLogger();

            public static void Error(Exception ex)
            {
                System.Diagnostics.Trace.TraceError(ex.ToString());
                //throw ex;
                _logger.Error(ex);
            }
            public static void Error(string ex)
            {
                System.Diagnostics.Trace.TraceError(ex.ToString());
                _logger.Error(ex);
            }

            public static void Info(string message)
            {
                _logger.Info(message);
            }
            public static void Data(string subject, object data)
            {
                data = $"{subject}, {JsonConvert.SerializeObject(data)} ";
                _logger.Info(data);
            }
        }


    }
}