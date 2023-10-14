using ElmahCore;
using StackExchange.Profiling.Internal;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace LifeStyle.Services
{
    public class TwilioService
    {
        public static AppSettingService service;
        public static string accountSid;
        public static string authToken;
        public static string from;
        public TwilioService()
        {
            service = new AppSettingService();
            accountSid = service.GetWithDetails("Twilio.AccountSid");
            authToken = service.GetWithDetails("Twilio.AuthToken");
            from = service.GetWithDetails("Twilio.From");
        }
        public bool SendSms(string toMobileNumber, string content)
        {
            Common.Log.Data($"Sending Twilio SMS ..", $"MobileNo. - {toMobileNumber}, Body - {content}");
            try
            {
                TwilioClient.Init(accountSid, authToken);
                var msg = MessageResource.Create(
                    body: content,
                    from: new Twilio.Types.PhoneNumber(from),
                    to: new Twilio.Types.PhoneNumber(toMobileNumber)
                );
                return !msg.Sid.IsNullOrWhiteSpace();
            }
            catch (Exception ex)
            {
                ElmahExtensions.RiseError(ex);
            }
            return false;
        }

    }
}
