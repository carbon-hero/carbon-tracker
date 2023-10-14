using Core.Extensions;
using Core.Models;
using Core.RequestModel;
using Core.ResponseModels;
using ElmahCore;
using FluentValidation;
using LifeStyle.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace LifeStyle.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("CorsPolicy")]
    public class UserPointTransactionsController : ControllerBase
    {
        [HttpGet("Get")]
        public ApiResponse Get([Required] int id)
        {
            var res = new ApiResponse();
            try
            {
                var me = TokenService.GetUserByToken(Request.Headers);
                var data = Common.Instances.UserPointTransactionInst.Get(id);
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

        [HttpGet("CheckMyBalance")]
        public ApiResponse CheckMyBalance()
        {
            var res = new ApiResponse();
            try
            {
                var me = TokenService.GetUserByToken(Request.Headers);
                var data = Common.Instances.UserPointTransactionInst.CheckBalance(me.Id, 0);
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
        public ApiResponse GetAll(int pageNumber = 1, int pageSize = 20)
        {
            var res = new ApiResponse();
            try
            {
                var me = TokenService.GetUserByToken(Request.Headers);
                res.Data = Common.Instances.UserPointTransactionInst.GetAll(pageNumber, pageSize, me.Id);
            }
            catch (Exception ex)
            {
                HttpContext.RiseError(ex);
                res.PrepareErrorResponse(ex);
            }
            return res;
        }

        [HttpPost("Save")]
        public ApiResponse Save(UserPointTransactionRequest req)
        {
            var me = TokenService.GetUserByToken(Request.Headers);
            var res = new ApiResponse();
            try
            {
                var validator = new InlineValidator<UserPointTransactionRequest>();
                validator.RuleSet("UserPointTransactionValidator", () =>
                {
                    validator.RuleFor(x => x.TransactionType).NotNull().NotEmpty();
                    validator.RuleFor(x => x.Points).Must(x=>x> 0);
                });
                var valRes = validator.Validate(req, ruleSet: "UserPointTransactionValidator");
                if (!valRes.IsValid)
                {
                    return res.PrepareInvalidRequest(ref valRes);
                }
                res.Data = Common.Instances.UserPointTransactionInst.Points(req, me.Id);
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
                res.Data = Common.Instances.UserPointTransactionInst.Delete(id);
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