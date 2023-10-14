using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.ResponseModels
{
    public class ApiResponse
    {
        private object _data;

        public ApiResponse()
        {
            Data = new object();
            Message = Constants.ResponseMessage.ok;
            StatusCode = StatusCodes.Status200OK;
        }

        public object Data
        {
            get => _data;
            set
            {
                _data = value;
                PrepareResponse();
            }
        }

        public string Message { get; set; }
        public int StatusCode { get; set; }
        public object Errors { get; set; }

        public void PrepareResponse()
        {
            Message = Data != null ? Constants.ResponseMessage.ok : Constants.ResponseMessage.BadRequest;
            StatusCode = Data != null ? StatusCodes.Status200OK : StatusCodes.Status400BadRequest;
        }

        public void PrepareErrorResponse(Exception ex)
        {
            Message = ex.Message;
            StatusCode = StatusCodes.Status500InternalServerError;
            if(ex.Message == "Attempted to perform an unauthorized operation.")
                StatusCode = StatusCodes.Status401Unauthorized;
        }
    }

}
