using System;
using System.IO;

namespace Core
{
    public class Constants
    {
        public class DataTypes
        {
            public static string TravelMode => "TravelMode";
            public static string TravelType => "TravelType";
        }


        public static readonly string KeyLOGINOTP = "LOGINOTP";
        public static string[] mediaExtensions = {
         ".WAV", ".MID", ".MIDI", ".WMA", ".MP3", ".OGG", ".RMA",
         ".AVI", ".MP4", ".DIVX", ".WMV",".MOV", ".GPX",
         };

        public static string[] thumbnailExtensions = { ".JPEG", ".JPG", ".PNG", ".GIF" };

        public static bool IsMediaFile(string path)
        {
            return -1 != Array.IndexOf(mediaExtensions, Path.GetExtension(path).ToUpperInvariant());
        }

        public static bool IsThumbnailFile(string path)
        {
            return -1 != Array.IndexOf(thumbnailExtensions, Path.GetExtension(path).ToUpperInvariant());
        }

        public const string DefaultUserApiKey = "c63fce44-0dc7-44d3-ba29-0fc04808f68d";
        public static readonly string LogoUrl = "";

        public static readonly string DefaultAdminEmail = "ezbookit@mailspons.com";
        public static readonly string DefaultMode = "dev";
        public static readonly string DefaultHost = "smtp.mailspons.com";
        public static readonly string DefaultUserName = "ezbookit";
        public static readonly string DefaultPassword = "5677e9424af0485a991493e07d5f5e1a";
        public static readonly string DefaultPort = "587";
        public static readonly string DefaultFrom = "ezbookit@mailspons.com";
        public static readonly string DefaultReplyTo = "ezbpartners@gmail.com";
        public static readonly string Secret = "Secret";
        public static readonly string DefaultSecret = "01F9EX1GFFBD441HDQMB7ER6X4";

        public static readonly string UploadFolder = "AppData";
        public static readonly string RoleUser = "User";
        public static readonly int RoleUserId = 2;

        public static readonly string KeyForgotPassword = "FORGOTPASSWORD";
        public static readonly string KeyChangePassword = "CHANGEPASSWORD";
        public static readonly string AdminEmail = "AdminEmail";
        public static readonly string AdminSiteUrl = "AdminSiteUrl";
        public static readonly string KeyWelcome = "WELCOME";
        public static readonly string KeyVERIFYBYOTP = "VERIFYBYOTP";
        public static readonly string KeyNewDeviceLogin = "NEWDEVICELOGIN";
        public static readonly string KeyContentReport = "CONTENTREPORT";
        public static readonly string SupportEmail = "SupportEmail";

        public static class ResponseMessage
        {
            public static readonly string AlreadyFollowed = "Your are already follow this user!";
            public static readonly string PasswordChanged = "Password changed successfully";
            public static readonly string ErrorPrivateAccount = "This is private account, you are not able to follow";
            public static readonly string ErrorThumbnail = "Please upload thumbnail image for this video";
            public static readonly string UserBlockedMsg = "User blocked";
            public static readonly string UserUnBlockedMsg = "User Unblocked";
            public static readonly string AlreadyBlocked = "Already Blocked!";
            public static readonly string ErrorTags = "Please enter valid tag, ex:- Chat, Favourite ";
            public static readonly string TokenExpired = "Facebook Token has expired!";
            public static readonly string ReportImage = "Thanks for reporting! We would look into it.";
            public static readonly string ErrorAddresses = "Please enter valid address";
            public static readonly string ErrorVideoFormat = "This video format not uploded";
            public static readonly string ErrorOTP = "Invalid OTP!";
            public static readonly string UserNotFoundLIMS = "User not found on LIMS";
            public static readonly string ErrorSendEmail = "error in sent email!";
            public static readonly string SendEmail = "Email sent Successfully!";
            public static readonly string ErrorPasswordNotMatch = "Your old password is incorrect.";
            public static readonly string NotRegistered = "If there is an account associated with that email, instructions will be sent for how to reset the password";
            public static readonly string NotDeletedDefaultClinicianId = "You are not able to delete this user";
            public static readonly string TryAgain = "This reset link as already been used";
            public static readonly string PhoneNumberNotFound = "Phone Number Not Found";
            public static readonly string ErrorAddressType = "Please enter valid address type";
            public static readonly string ErrorGender = "Please enter valid gender";
            public static readonly string ErrorLogin = "Your session is expired! Please login again";

            public static readonly string ok = "Successful";    //GetConstant("ok");//"Successful";
            public static readonly string Already = "Already registered";
            public static readonly string Unauthorized = "401";
            public static readonly string BadRequest = "Bad Request";
            public static readonly string NotFound = "Not Found";
            public static readonly string InternalServerError = "Internal server error";
            public static readonly string Delete = "Your data has been successfully deleted!";
            public static readonly string InvalidDateTime = "Please enter a Valid date Format like ex.yyyy-MM-ddTHH:mm:ssZ";
            public static readonly string InvalidDate = "Please enter a Valid date Format like ex.yyyy-MM-dd";
            public static readonly string TimeError = "Please enter correct time using 24 hour format";
            public static readonly string PermissionRequired = "Permission Required !";
            public static readonly string InvalidCredentials = "Invalid credentials !";
            public static readonly string UserBlocked = "User is Blocked";
            public static readonly string TemplateNotFound = "Template Not Found";
            public static readonly string UserNotFound = "User Not Found";
            public static readonly string UserNotFoundOnStripe = "User Not Found on Stripe";
            public static readonly string AddressNotFound = "Address Not Found";
            public static readonly string PromoCodeNotFound = "Promo Code is invalid";
            public static readonly string PromoCodeExpired = "Promo Code is expired";
            public static readonly string OrderNotFound = "Order Not Found";
            public static readonly string ShippingAddressNotFound = "Shipping Address Not Found";
            public static readonly string BillingAddressNotFound = "Billing Address Not Found";
            public static readonly string ProductNotFound = "Product Not Found";
            public static readonly string ErrorAddSquare = "Unable to add on square";
            public static readonly string ErrorPlaceOrder = "Please complete order process";
            public static readonly string ErrorOrderStatus = "Please enter valid order status";
            public static readonly string ErrorEmailVerify = "Please verify your account before proceeding";
            public static readonly string EmailVerify = "Thanks for validating your email, redirecting to login page now";
            public static readonly string EmailVerifyByOTP = "Thanks for validating your email";
            public static readonly string InvalidEthnicity = "Please enter a valid ethnicity";
            public static readonly string SelectEthnicity = "Please select at least one ethnicity";
            public static readonly string ErrorRejectionReason = "Please enter rejection reason";
            public static readonly string AlreadyVerified = "your email address already verified";
            public static readonly string ErrorAddress = "Please enter valid zipCode and city";
            public static readonly string SubscriptionNotFound = "Subscription Not Found";
            public static readonly string ErrorEmailAddress = "Please enter email address";
            public static readonly string ErrorFileType = "Please enter valid file type";
            public static readonly string AlreadyLike = "You are already like this post!";
            public static readonly string ErrorReason = "Please enter a report reason";
            public static readonly string ErrorReportType = "Please enter valid report type";
            public static readonly string AlreadyReported = "You have already reported this content!";
            public static readonly string ErrorCategoryType = "Please enter valid Category type, ex:- INTERESTS, RECOMMENDED";
            public static readonly string ErrorNotificationType = "Please enter valid notification type, ex:- FOLLOW, MESSAGE";
            public static readonly string AlreadyNotify = "You have already notify!";

            public static readonly string ErrorReport = "you are not able to report own content!";
        }

        public static readonly string[] Gender = new string[] { "Male", "Female" }; //"Other", "Non-Binary", "Not Specified"
        public static readonly string[] OrderStatuses = new string[] { "Order Received", "Order Kit in Transit", "Test Kit Registered", "Sample in Transit to Lab", "Sample Received", "Result" };

        public static readonly string Rejected = "Rejected";
        public static readonly string Canceled = "Canceled";

        public static readonly string[] Tag = new string[] { "Chat", "Favourite" };

        public static string[] FileTypes => new string[] { "image", "video" };

        public static string ErrorUserRcId(string rcId)
        {
            return $"{rcId} RC User Id Not Found";
        }

        public static Exception ErrorAlready(string type)
        {
            return new Exception($"You already have the {type}!");
        }

        public static Exception ErrorFormatType(string type, string format)
        {
            return new Exception($"This {type} {format} format is not uploded, please upload valid format files");
        }

        public static class Modes
        {
            public static string Dev => "dev";
            public static string Pro => "pro";
        }


        public static class Roles
        {
            public static string Admin => "Admin";
            public static string User => "User";
            public static string Other => "Other";
        }
        public static class FileType
        {
            public static string Image => "image";
            public static string Video => "video";
        }

        public static Exception Invalid(string key)
        {
            return new Exception($"{key} not valid. Please check your {key}.");
        }

        public static Exception InvalidMobileNumber()
        {
            return new Exception($"Mobile number not valid. Please check your mobile number.");
        }

        public static Exception ErrorSetting(string key)
        {
            return new Exception($"{key} setting not found");
        }
        public static Exception ErrorNotFound(string type)
        {
            return new Exception($"This {type} Id is Not Found, Please enter valid {type} Id");
        }

        public enum TransactionType
        {
            Earned,
            Spent
        }
    }
}