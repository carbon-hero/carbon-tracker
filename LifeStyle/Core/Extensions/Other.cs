using Core.ResponseModels;
using FluentValidation.Results;
using System;
using System.Linq;

namespace Core.Extensions
{
    public static class Other
    {
        public static ApiResponse PrepareInvalidRequest(this ApiResponse res, ref ValidationResult valRes)
        {
            res.Data = null;
            res.Errors = "Invalid Request";
            res.Message = valRes.Errors.FirstOrDefault().ErrorMessage; 
            return res;
        }

        public static string FirstCharToUpper(this string input)
        {
            switch (input)
            {
                case null: throw new ArgumentNullException(nameof(input));
                case "": throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input));
                default: return input[0].ToString().ToUpper() + input.Substring(1);
            }
        }

        public static Decimal Round(this Decimal d)
        {
            return Math.Round(d, 2);
        }

        public static string GetUntilOrEmpty(this string text, string stopAt = "-")
        {
            if (!String.IsNullOrWhiteSpace(text))
            {
                int charLocation = text.IndexOf(stopAt, StringComparison.Ordinal);

                if (charLocation > 0)
                {
                    return text.Substring(0, charLocation);
                }
            }

            return String.Empty;
        }
        public static Exception PrepareInvalidRequest(string keyName)
        {
            return new Exception($"{keyName} must not be empty or null");
        }

        public static string LikeCountFormatted(this int likeCount)
        {
            return likeCount > 0 ? String.Format("{0:#,###}", likeCount) : "0";
        }
    }
}