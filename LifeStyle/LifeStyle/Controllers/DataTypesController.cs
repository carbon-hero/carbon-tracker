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
using StackExchange.Profiling.Internal;
using System.ComponentModel.DataAnnotations;

namespace LifeStyle.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("CorsPolicy")]
    public class DataTypesController : ControllerBase
    {
        private DataTypeService service;
        public DataTypesController() 
        {
            service = new DataTypeService();
        }

        [HttpGet("GetDataType")]
        public ApiResponse GetDataType([Required] int id)
        {
            var res = new ApiResponse();
            try
            {
                var me = TokenService.GetUserByToken(Request.Headers);
                var data = Common.Instances.DataTypeInst.Get(id);
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

        [HttpGet("GetAllDataType")]
        public ApiResponse GetAllDataType(int pageNumber = 1, int pageSize = 20, string? query = "")
        {
            var res = new ApiResponse();
            try
            {
                var me = TokenService.GetUserByToken(Request.Headers);
                var data = Common.Instances.DataTypeInst.GetAll(pageNumber, pageSize, query);
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

        [HttpGet("GetAllDataTypeValue")]
        public ApiResponse GetAllDataTypeValue(string? typeName, string? query)
        {
            var res = new ApiResponse();
            try
            {
                var me = TokenService.GetUserByToken(Request.Headers);
                var data = Common.Instances.DataTypeValuesInst.GetAll(typeName, query);
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
        /// Save data type on db
        /// </summary>
        /// <returns></returns>

        [HttpPost("SaveDataType")]
        public ApiResponse SaveDataType(DataTypeRequest req)
        {
            var res = new ApiResponse();
            try
            {
                var me = TokenService.GetUserByToken(Request.Headers);
                var validator = new InlineValidator<DataTypeRequest>();
                validator.RuleSet("DataTypeValidator", () =>
                {
                    validator.RuleFor(x => x.Name).NotNull().NotEmpty();
                });
                var valRes = validator.Validate(req, ruleSet: "DataTypeValidator");
                if (!valRes.IsValid)
                    return res.PrepareInvalidRequest(ref valRes);
                res.Data = service.SaveDataType(req, me.Id);
            }
            catch (Exception ex)
            {
                HttpContext.RiseError(ex);
                res.PrepareErrorResponse(ex);
            }
            return res;
        }
        /// <summary>
        /// Save data type value on db
        /// </summary>
        /// <returns></returns>

        [HttpPost("SaveDataTypeValue")]
        public ApiResponse SaveDataTypeValue(DataTypeValueRequest req)
        {
            var res = new ApiResponse();
            try
            {
                var me = TokenService.GetUserByToken(Request.Headers);
                var validator = new InlineValidator<DataTypeValueRequest>();
                validator.RuleSet("DataTypeValueValidator", () =>
                {
                    validator.RuleFor(x => x.Value).NotNull().NotEmpty();
                    validator.RuleFor(x => x.TypeId).Must(x => x > 0);
                });
                var valRes = validator.Validate(req, ruleSet: "DataTypeValueValidator");
                if (!valRes.IsValid)
                    return res.PrepareInvalidRequest(ref valRes);
                res.Data = service.SaveDataTypeValue(req, me.Id);
            }
            catch (Exception ex)
            {
                HttpContext.RiseError(ex);
                res.PrepareErrorResponse(ex);
            }
            return res;
        }

        /// <summary>
        /// Delete data type by Id from db
        /// </summary>
        /// <returns></returns>

        [HttpDelete("DeleteDataType")]
        public ApiResponse DeleteDataType([Required] int id)
        {
            var res = new ApiResponse();
            try
            {
                var me = TokenService.GetUserByToken(Request.Headers);
                res.Data = Common.Instances.DataTypeInst.Delete(id);
            }
            catch (Exception ex)
            {
                HttpContext.RiseError(ex);
                res.PrepareErrorResponse(ex);
            }
            return res;
        }

        /// <summary>
        /// Delete data type value by Id from db
        /// </summary>
        /// <returns></returns>

        [HttpDelete("DeleteDataTypeValue")]
        public ApiResponse DeleteDataTypeValue([Required] int id)
        {
            var res = new ApiResponse();
            try
            {
                var me = TokenService.GetUserByToken(Request.Headers);
                res.Data = Common.Instances.DataTypeValuesInst.Delete(id);
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

