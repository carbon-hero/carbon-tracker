using JWT.Algorithms;
using JWT.Builder;
using JWT.Exceptions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using System;
using Core;
using Core.Models;
using LifeStyle;

namespace LifeStyle.Services
{
    public static class TokenService
    {
        private static string Secret => new Utility.AppConfig().GetValue(Constants.Secret, Constants.DefaultSecret);

        public static User GetUserByToken(IHeaderDictionary headers)
        {
            var token = headers["Authorization"];
            token = token.ToString().Replace("Bearer", "").Trim();
            var user = GetByToken(token);
            if (user == null || user.Id <= 0)
                throw new UnauthorizedAccessException();
            return user;
        }

        public static string GenerateToken(int id)
        {
            var token = JwtBuilder.Create()
                .WithAlgorithm(new HMACSHA256Algorithm()) // symmetric
                .WithSecret(Secret)
                .AddClaim("exp", DateTimeOffset.UtcNow.AddYears(30).ToUnixTimeSeconds())
                .AddClaim("id", id)
                .Encode();
            return token;
        }


        private static User GetByToken(string token)
        {
            try
            {
                if (token == "")
                {
                    throw new UnauthorizedAccessException();
                }
                var json = JwtBuilder.Create()
                    .WithAlgorithm(new HMACSHA256Algorithm()) // symmetric
                    .WithSecret(Secret)
                    .MustVerifySignature()
                    .Decode(token);
                dynamic data = JObject.Parse(json);
                int id = data.id;
                var userdata = Common.Instances.UserInst.Get(id);
                return userdata;
            }
            catch (TokenExpiredException)
            {
                throw new TokenExpiredException("Token has expired");
            }
            catch (SignatureVerificationException)
            {
                throw new SignatureVerificationException("Token has invalid signature");
            }
        }
    }
}


