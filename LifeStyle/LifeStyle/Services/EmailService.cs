using ElmahCore;
using StackExchange.Profiling.Internal;
using static Core.Utility;
using System.Net.Mail;
using System.Net;
using System.Text;
using System;
using Core;
using LifeStyle;

namespace LifeStyle.Services
{
    public class EmailService
    {

        public static AppSettingService service;
        public static string mode;
        public static string key;
        public static string host;
        public static int port;
        public static string userName;
        public static string password;
        public static string formEmail;
        public static string replyTo;

        public EmailService()
        {
            service = new AppSettingService();
            mode = service.GetWithDetails("Email.Mode");
            key = mode == "dev" ? "staging_mail" : "smtp";
            host = service.GetWithDetails("Email.Host");
            port = Convert.ToInt32(service.GetWithDetails("Email.Port"));
            userName = service.GetWithDetails("Email.UserName");
            password = service.GetWithDetails("Email.Password");
            formEmail = service.GetWithDetails("Email.FormEmail");
            replyTo = service.GetWithDetails("Email.ReplyTo");
        }


        public bool SendEmail(string mailTo, string mailSubject, bool mailIsHtml, string body, string bcc, bool replyto = false)
        {
            System.Net.ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
            Common.Log.Info($"Sending email..");
            try
            {
                if (replyto) replyTo = new AppConfig().GetValue(key, "replyTo", Constants.DefaultReplyTo);
                if (!bcc.IsNullOrWhiteSpace()) bcc = new AppConfig().GetValue(key, bcc, Constants.DefaultAdminEmail);
                if (mailTo == "" || mailSubject == "" || body == "" || formEmail == "")
                    return false;

                var smtp = new SmtpClient(host)
                {
                    Port = port,
                    UseDefaultCredentials = false,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new NetworkCredential(userName, password)
                };
                var mailMessage = new MailMessage
                {
                    From = new MailAddress(formEmail, "Email"),
                    Subject = mailSubject,
                    BodyEncoding = Encoding.UTF8,
                    IsBodyHtml = mailIsHtml,
                    Body = body
                };
                if (!bcc.IsNullOrWhiteSpace()) mailMessage.Bcc.Add(bcc);
                mailMessage.To.Add(new MailAddress(mailTo));
                //mailMessage.ReplyToList.Add(new MailAddress(replyTo));
                smtp.Send(mailMessage);
                mailMessage.Attachments.Dispose();
                smtp.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                ElmahExtensions.RiseError(ex);
                return false; //throw ex;
            }
        }

    }
}

