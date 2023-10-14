using ElmahCore;
using FluentValidation;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System;
using Core.ResponseModels;
using Core.RequestModel;
using LifeStyle.Services;
using Core.Extensions;

namespace LifeStyle.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("CorsPolicy")]
    public class TemplateController : ControllerBase
    {
        /// <summary>
        /// Get template by Id from db
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpGet("Get")]
        public ApiResponse Get([Required] int id)
        {
            var res = new ApiResponse();
            try
            {
                var me = TokenService.GetUserByToken(Request.Headers);
                //if (me.IsAdmin)
                //    throw new UnauthorizedAccessException();
                var data = Common.Instances.TemplateInst.Get(id);
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
        /// Get all template from db
        /// </summary>
        /// <returns></returns>

        [HttpGet("GetAll")]
        public ApiResponse GetAll(int pageNumber = 1, int pageSize = 20, string? query = "all")
        {
            var res = new ApiResponse();
            try
            {
                var me = TokenService.GetUserByToken(Request.Headers);
                //if (!me.IsAdmin)
                //    throw new UnauthorizedAccessException();
                res.Data = Common.Instances.TemplateInst.GetAll(pageNumber, pageSize, query);
            }
            catch (Exception ex)
            {
                HttpContext.RiseError(ex);
                res.PrepareErrorResponse(ex);
            }
            return res;
        }

        /// <summary>
        /// Save template on db
        /// </summary>
        /// <returns></returns>

        [HttpPost("Save")]
        public ApiResponse Save(TemplateRequest req)
        {
            var res = new ApiResponse();
            try
            {
                var me = TokenService.GetUserByToken(Request.Headers);
                var validator = new InlineValidator<TemplateRequest>();
                validator.RuleSet("TemplateValidator", () =>
                {
                    validator.RuleFor(x => x.Key).NotNull().NotEmpty();
                    validator.RuleFor(x => x.Content).NotNull().NotEmpty();
                    validator.RuleFor(x => x.Subject).NotNull().NotEmpty();
                });
                var valRes = validator.Validate(req, ruleSet: "TemplateValidator");
                if (!valRes.IsValid)
                {
                    return res.PrepareInvalidRequest(ref valRes);
                }
                res.Data = new TemplateService().Save(req, me.Id);
            }
            catch (Exception ex)
            {
                if (ex.Message == $"Duplicate entry '{req.Key}' for key 'UK_Templates_Key'")
                {
                    res.Data = null;
                    res.Message = $"The Template Key {req.Key} is already taken.";
                    res.StatusCode = StatusCodes.Status409Conflict;
                    return res;
                }
                else
                {

                    HttpContext.RiseError(ex);
                    res.PrepareErrorResponse(ex);
                }
            }
            return res;
        }

        /// <summary>
        /// Delete template by Id from db
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
                res.Data = Common.Instances.TemplateInst.Delete(id);
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

