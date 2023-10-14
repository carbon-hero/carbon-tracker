using LifeStyle.Services;
using Core.ResponseModels;
using ElmahCore;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NPoco;
using Org.BouncyCastle.Ocsp;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;

namespace LifeStyle.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("CorsPolicy")]
    public class UtilityController : ControllerBase
    {
       
        [HttpGet("GetPlaceDetails")]
        public async Task<ApiResponse> GetPlaceDetails([Required] string placeId)
        {
            var res = new ApiResponse();
            try
            {
                res.Data = GoogleService.GetPlaceDetails(placeId);
            }
            catch (Exception ex)
            {
                HttpContext.RiseError(ex);
                res.PrepareErrorResponse(ex);
            }
            return res;
        }

        [HttpGet("GetLocationInfo")]
        public async Task<ApiResponse> GetLocationInfo([Required] double lat, [Required] double log)
        {
            var res = new ApiResponse();
            try
            {
                res.Data = await new GoogleService().GetLocationInfo(lat, log);
            }
            catch (Exception ex)
            {
                HttpContext.RiseError(ex);
                res.PrepareErrorResponse(ex);
            }
            return res;
        }

        [HttpGet("GetFullMapRouteMapUrl")]
        public async Task<ApiResponse> GetFullMapRouteMapUrl(double fromLat, double fromLong, double toLat, double toLong)
        {
            var res = new ApiResponse();
            try
            {
                res.Data = GoogleService.GetFullMapRouteMapUrl(fromLat, fromLong, toLat, toLong);
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

