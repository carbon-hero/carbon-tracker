using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System;
using Core.ResponseModels;
using ElmahCore;
using FluentValidation;
using StackExchange.Profiling.Internal;
using LifeStyle.Services;
using Core.Extensions;
using Core.RequestModel;

namespace LifeStyle.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("CorsPolicy")]
    public class AppSettingController : ControllerBase
    {  
        /// <summary>
       /// Get setting by Id from db
       /// </summary>
       /// <returns></returns>

        [HttpGet("Get")]
        public ApiResponse Get([Required] int id)
        {
            var res = new ApiResponse();
            try
            {
                var me = TokenService.GetUserByToken(Request.Headers);
                //if (!me.IsAdmin)
                //    throw new UnauthorizedAccessException();
                var data = Common.Instances.AppSettingInst.GetWithDetails(id);
                Common.FixNull(ref data);
                res.Data = data;
            }
            catch (Exception ex)
            {
                HttpContext.RiseError(ex);
                res.PrepareErrorResponse(ex);
            }
            return res;
        }

        /// <summary>
        /// Get All setting from db
        /// </summary>
        /// <returns></returns>

        [HttpGet("GetAll")]
        public ApiResponse GetAll(int pageNumber = 1, int pageSize = 20, string? query = "")
        {
            var res = new ApiResponse();
            try
            {
                var token = Request.Headers["Authorization"];
                string tokenExist = token.ToString().Replace("Bearer", "").Trim();
                if (tokenExist.IsNullOrWhiteSpace())
                {
                    res.Data = Common.Instances.AppSettingInst.GetAll(pageNumber, pageSize, query);
                    return res;
                }
                var me = TokenService.GetUserByToken(Request.Headers);
                //if (!me.IsAdmin)
                //    throw new UnauthorizedAccessException();
                res.Data = Common.Instances.AppSettingInst.GetAll(pageNumber, pageSize, query, false);
            }
            catch (Exception ex)
            {
                HttpContext.RiseError(ex);
                res.PrepareErrorResponse(ex);
            }
            return res;
        }

        /// <summary>
        /// Save setting on db
        /// </summary>
        /// <returns></returns>

        [HttpPost("Save")]
        public ApiResponse Save(AppSettingRequest model)
        {
            var me = TokenService.GetUserByToken(Request.Headers);
            var res = new ApiResponse();
            try
            {
                var validator = new InlineValidator<AppSettingRequest>();
                validator.RuleSet("AppSettingValidator", () =>
                {
                    validator.RuleFor(x => x.Key).NotNull().NotEmpty();
                    validator.RuleFor(x => x.Value).NotNull().NotEmpty();
                });
                var valRes = validator.Validate(model, ruleSet: "AppSettingValidator");
                if (!valRes.IsValid)
                {
                    return res.PrepareInvalidRequest(ref valRes);
                }
                res.Data = new AppSettingService().Save(model, me.Id);
            }
            catch (Exception ex)
            {
                HttpContext.RiseError(ex);
                res.PrepareErrorResponse(ex);
            }
            return res;
        }

        /// <summary>
        /// Delete setting by Id from db
        /// </summary>
        /// <returns></returns>

        [HttpDelete("Delete")]
        public ApiResponse Delete([Required] int id)
        {
            var res = new ApiResponse();
            try
            {
                var me = TokenService.GetUserByToken(Request.Headers);
                //if (!me.IsAdmin)
                //    throw new UnauthorizedAccessException();
                res.Data = Common.Instances.AppSettingInst.Delete(id);
            }
            catch (Exception ex)
            {
                HttpContext.RiseError(ex);
                res.PrepareErrorResponse(ex);
            }
            return res;
        }
    }
}

