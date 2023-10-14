using LifeStyle.Services;
using Core.Extensions;
using Core.RequestModel;
using Core.ResponseModels;
using ElmahCore;
using FluentValidation;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace LifeStyle.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("CorsPolicy")]
    public class CommuteHistoryController : ControllerBase
    {
        private CommuteHistoryService service;
        public CommuteHistoryController() 
        {
            service = new CommuteHistoryService();
        }

        [HttpGet("Get")]
        public ApiResponse Get([Required] int id)
        {
            var res = new ApiResponse();
            try
            {
                var me = TokenService.GetUserByToken(Request.Headers);
                var data = service.Get(id);
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
                res.Data = service.GetAll(pageNumber, pageSize, me.Id, query);
            }
            catch (Exception ex)
            {
                HttpContext.RiseError(ex);
                res.PrepareErrorResponse(ex);
            }
            return res;
        }

        [HttpPost("Save")]
        public ApiResponse Save(CommuteHistoryRequest req)
        {
            var me = TokenService.GetUserByToken(Request.Headers);
            var res = new ApiResponse();
            try
            {
                var validator = new InlineValidator<CommuteHistoryRequest>();
                validator.RuleSet("CommuteHistoryValidator", () =>
                {
                    validator.RuleFor(x => x.FromPlaceId).NotNull().NotEmpty();
                    validator.RuleFor(x => x.ToPlaceId).NotNull().NotEmpty();
                    validator.RuleFor(x => x.TypeId).Must(x => x > 0);
                    validator.RuleFor(x => x.ModeId).Must(x => x > 0);
                    validator.RuleFor(x => x.FromLat).Must(x => x > 0);
                    validator.RuleFor(x => x.FromLong).Must(x => x > 0);
                    validator.RuleFor(x => x.ToLat).Must(x => x > 0);
                    validator.RuleFor(x => x.ToLong).Must(x => x > 0);
                });
                var valRes = validator.Validate(req, ruleSet: "CommuteHistoryValidator");
                if (!valRes.IsValid)
                {
                    return res.PrepareInvalidRequest(ref valRes);
                }
                res.Data = new CommuteHistoryService().Save(req, me.Id);
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
                res.Data = Common.Instances.CommuteHistoryInst.Delete(id);
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