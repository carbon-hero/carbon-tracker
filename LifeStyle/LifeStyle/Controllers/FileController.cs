using ElmahCore;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System;
using Core.ResponseModels;
using LifeStyle.Services;
using Core.RequestModel;
using System.Collections.Generic;
using FluentValidation;
using Core;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using AgileObjects.AgileMapper.Extensions;
using Core.Extensions;

namespace LifeStyle.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("CorsPolicy")]
    public class FileController : ControllerBase
    {
        private FileService service;
        public FileController() 
        { 
            service = new FileService();
        }

        /// <summary>
        /// Get single by id from db
        /// </summary>
        /// <returns></returns>
        [HttpGet("Get")]
        public ApiResponse Get([Required] int id)
        {
            var res = new ApiResponse();
            try
            {
                var data = Common.Instances.FileInst.Get(id);
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
        public ApiResponse GetAll(int pageNumber = 1, int pageSize = 20, string? query = "")
        {
            var res = new ApiResponse();
            try
            {
                var me = TokenService.GetUserByToken(Request.Headers);
                //if (!me.IsAdmin)
                //    throw new UnauthorizedAccessException();
                res.Data = Common.Instances.FileInst.GetAll(pageNumber, pageSize, query);
            }
            catch (Exception ex)
            {
                HttpContext.RiseError(ex);
                res.PrepareErrorResponse(ex);
            }
            return res;
        }


        /// <summary>
        /// Save in db
        /// </summary>
        /// <returns></returns>
        [HttpPost("Save")]
        public ApiResponse Save([FromForm] FileRequest request)
        {
            var res = new ApiResponse();
            try
            {
                var me = TokenService.GetUserByToken(Request.Headers);
                var validator = new InlineValidator<FileRequest>();
                validator.RuleSet("FileValidator", () =>
                {
                    validator.RuleFor(x => x.File).NotNull().NotEmpty();
                    validator.RuleFor(x => x.Type).NotNull().NotEmpty().Must(x => Constants.FileTypes.Contains(x))
                  .WithMessage(Constants.ResponseMessage.ErrorFileType);
                 
                });
                var valRes = validator.Validate(request, ruleSet: "FileValidator");
                if (!valRes.IsValid)
                    return res.PrepareInvalidRequest(ref valRes);

                if(request.Type == Constants.FileType.Video && !Constants.IsMediaFile(request.File.FileName))
                    throw Constants.ErrorFormatType("Video", new FileInfo(request.File.FileName).Extension);

                res.Data = service.Upload(request, me.Id);
            }
            catch (Exception ex)
            {
                HttpContext.RiseError(ex);
                res.PrepareErrorResponse(ex);
            }
            return res;
        }

        /// <summary>
        /// Save multiple files in db
        /// </summary>
        /// <returns></returns>
        [HttpPost("SaveMultiple")]
        public ApiResponse SaveMultiple([FromForm] MultipleFileRequest req)
        {
            var res = new ApiResponse();
            try
            {
                var me = TokenService.GetUserByToken(Request.Headers);
                var files = new List<Core.Models.File>();
                var validator = new InlineValidator<MultipleFileRequest>();
                validator.RuleSet("FileValidator", () =>
                {
                    validator.RuleFor(x => x.Type).NotNull().NotEmpty().Must(x => Constants.FileTypes.Contains(x))
                  .WithMessage(Constants.ResponseMessage.ErrorFileType);
                });
                var valRes = validator.Validate(req, ruleSet: "FileValidator");
                if (!valRes.IsValid)
                    return res.PrepareInvalidRequest(ref valRes);

                foreach (var file in req.File)
                {
                    var request = new Core.RequestModel.FileRequest()
                    {
                        Type = req.Type,
                        File = file
                    };
                    request = req.Map().Over(request);
                    files.Add(service.Upload(request, me.Id)); 
                }
                res.Data = files;
            }
            catch (Exception ex)
            {
                HttpContext.RiseError(ex);
                res.PrepareErrorResponse(ex);
            }
            return res;
        }
        /// <summary>
        /// Delete file by id from db
        /// </summary>
        /// <returns></returns>
        [HttpDelete("Delete")]
        public ApiResponse Delete([Required] int id)
        {
            var res = new ApiResponse();
            try
            {
                var me = TokenService.GetUserByToken(Request.Headers);
                res.Data = Common.Instances.FileInst.Delete(id);
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

