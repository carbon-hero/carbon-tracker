using ElmahCore;
using FluentValidation;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System;
using Core.RequestModel;
using Core.ResponseModels;
using Core;
using LifeStyle.Services;
using Core.Extensions;

namespace LifeStyle.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("CorsPolicy")]
    public class SnippetController : ControllerBase
    {
        [HttpGet("Get")]
        public ApiResponse Get([Required] int id)
        {
            var res = new ApiResponse();
            try
            {
                var me = TokenService.GetUserByToken(Request.Headers);
                var data = Common.Instances.SnippetInst.Get(id);
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

        [HttpGet("GetAll")]
        public ApiResponse GetAll(int pageNumber = 1, int pageSize = 20, string? query = "all")
        {
            var res = new ApiResponse();
            try
            {
                var me = TokenService.GetUserByToken(Request.Headers);
                res.Data = Common.Instances.SnippetInst.GetAll(pageNumber, pageSize, query);
            }
            catch (Exception ex)
            {
                HttpContext.RiseError(ex);
                res.PrepareErrorResponse(ex);
            }
            return res;
        }

        [HttpPost("Save")]
        public ApiResponse Save(SnippetRequest req)
        {
            var me = TokenService.GetUserByToken(Request.Headers);
            var res = new ApiResponse();
            try
            {
                var validator = new InlineValidator<SnippetRequest>();
                validator.RuleSet("SnippetValidator", () =>
                {
                    validator.RuleFor(x => x.Key).NotNull().NotEmpty();
                    validator.RuleFor(x => x.Content).NotNull().NotEmpty();
                });
                var valRes = validator.Validate(req, ruleSet: "SnippetValidator");
                if (!valRes.IsValid)
                {
                    return res.PrepareInvalidRequest(ref valRes);
                }
                res.Data = new SnippetService().Save(req, me.Id);
            }
            catch (Exception ex)
            {
                HttpContext.RiseError(ex);
                res.PrepareErrorResponse(ex);
            }
            return res;
        }

        [HttpDelete("Delete")]
        public ApiResponse Delete([Required] int id)
        {
            var res = new ApiResponse();
            try
            {
                var me = TokenService.GetUserByToken(Request.Headers);
                //if (!me.IsAdmin)
                //    throw new UnauthorizedAccessException();
                res.Data = Common.Instances.SnippetInst.Delete(id);
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
