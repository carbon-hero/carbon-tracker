using AgileObjects.AgileMapper.Extensions;
using Core;
using Core.Models;
using Core.RequestModel;
using Core.ResponseModels;
using StackExchange.Profiling.Internal;

namespace LifeStyle.Services
{
    public class UserService
    {
       
        public User AddTreesPlanted(int userId)
        {
            var userDetails = Common.Instances.UserInst.Get(userId);
            if(userDetails.Id > 0)
            {
                userDetails.TreesPlanted += 1;
                Common.Instances.UserInst.Save(userDetails);
            }
            return userDetails;
        }

        public static string SendNotification(bool isSMS, string templateKey, User userDetail, string pass = "")
        {
            bool wasSent = false;
            string message = "";
            var getTemplate = Common.Instances.TemplateInst.GetByKey(templateKey);
            if (getTemplate.Content == null)
                throw new Exception(Constants.ResponseMessage.TemplateNotFound);
            var emailBody = getTemplate.Content;
            var bodyHtml = emailBody
                 .Replace("{Name}", $"{userDetail.FirstName} {userDetail.LastName}")
                 .Replace("{UserId}", userDetail.AddedBy.ToString())
                 .Replace("{Time}", DateTime.UtcNow.ToString("hh:mm"))
                 .Replace("{Date}", DateTime.UtcNow.ToString("yyyy-MMM-dd"))
                 .Replace("{Password}", pass.IsNullOrWhiteSpace() ? userDetail.Password.ToString() : pass)
                 .Replace("{LogoUrl}", Constants.LogoUrl);

            if (isSMS)
            {
                wasSent = new TwilioService().SendSms(userDetail.PhoneNumber, bodyHtml);
                message = (wasSent) ? "SMS sent Successfully!" : "error in sent sms!";
            }
           
            return message;
        }
        public User Signup(SignupRequest req, int meId)
        {
            var model = new User();
            var pass = req.Password;
            if (req.Id > 0) model = Common.Instances.UserInst.Get(req.Id);
             req.Password = req.Password.IsNullOrWhiteSpace() || req.Password == model.Password ? model.Password : Common.GetHash(req.Password);
            model = req.Map().Over(model);
            if (model.Id <= 0)
            {
                model.AddedOn = DateTime.UtcNow;
                model.AddedBy = meId;
            }
            model.ModifiedOn = DateTime.UtcNow;
            model.ModifiedBy = meId;
            var data = Common.Instances.UserInst.Save(model);
            return data;
        }

        public User SaveUser(UserRequest req, int meId)
        {
            var model = new User();
            var pass = req.Password;
            if (req.Id > 0) model = Common.Instances.UserInst.Get(req.Id);
            req.Password = req.Password.IsNullOrWhiteSpace() || req.Password == model.Password ? model.Password : Common.GetHash(req.Password);
            model = req.Map().Over(model);
            if (model.Id <= 0)
            {
                model.AddedOn = DateTime.UtcNow;
                model.AddedBy = meId;
            }
            model.ModifiedOn = DateTime.UtcNow;
            model.ModifiedBy = meId;
            var data = Common.Instances.UserInst.Save(model);
            return data;
        }


    }
}


