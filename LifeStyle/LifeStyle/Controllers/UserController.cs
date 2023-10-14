using ElmahCore;
using Core;
using Core.ResponseModels;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Core.RequestModel;
using System.ComponentModel.DataAnnotations;
using AgileObjects.AgileMapper.Extensions;
using Core.Extensions;
using Core.Models;
using FluentValidation;
using LifeStyle.Services;
using LifeStyle;
using Twilio.Rest.Autopilot.V1.Assistant;
using Org.BouncyCastle.Ocsp;

namespace TattoosAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("CorsPolicy")]
    public class UserController : ControllerBase
    {
        public UserService userService;
        public UserController()
        {
            userService = new UserService();
        }
        

        /// <summary>
        /// Get user profile by token (Header) from db
        /// </summary>
        /// <returns></returns>

        [HttpPost("Me")]
        public ApiResponse Me()
        {
            var res = new ApiResponse();
            try
            {
                var me = TokenService.GetUserByToken(Request.Headers);
                res.Data = Common.Instances.UserInst.Get(me.Id);
            }
            catch (Exception ex)
            {
                HttpContext.RiseError(ex);
                res.PrepareErrorResponse(ex);
            }
            return res;
        }

        /// <summary>
        /// Get user details by id from db
        /// </summary>
        /// <returns></returns>

        [HttpGet("Get")]
        public ApiResponse Get([Required] int id)
        {
            var res = new ApiResponse();
            try
            {
                var me = TokenService.GetUserByToken(Request.Headers);
                var data = Common.Instances.UserInst.Get(id);
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
        /// Get all user details from db
        /// </summary>
        /// <returns></returns>

        [HttpGet("GetAll")]
        public ApiResponse GetAll(int pageNumber = 1, int pageSize = 20, string? query = "", int userId = 0)
        {
            var res = new ApiResponse();
            try
            {
                var me = TokenService.GetUserByToken(Request.Headers);
                res.Data = Common.Instances.UserInst.GetAll(pageNumber, pageSize, query, userId);
            }
            catch (Exception ex)
            {
                HttpContext.RiseError(ex);
                res.PrepareErrorResponse(ex);
            }
            return res;
        }

        /// <summary>
        /// Get all my friends details from db
        /// </summary>
        /// <returns></returns>

        [HttpGet("GetAllMyFriends")]
        public ApiResponse GetAllMyFriends(int pageNumber = 1, int pageSize = 20, string? query = "", int userId = 0)
        {
            var res = new ApiResponse();
            try
            {
                var me = TokenService.GetUserByToken(Request.Headers);
                if (userId <= 0)
                    userId = me.Id;
                res.Data = Common.Instances.FriendInst.GetAll(pageNumber, pageSize, query, userId);
            }
            catch (Exception ex)
            {
                HttpContext.RiseError(ex);
                res.PrepareErrorResponse(ex);
            }
            return res;
        }

        /// <summary>
        /// Sign-Up  
        /// </summary>
        /// <returns></returns>


        [HttpPost("SignUp")]
        public ApiResponse SignUp(SignupRequest req)
        {
            var res = new ApiResponse();
            try
            {
               
                var validator = new InlineValidator<SignupRequest>();
                validator.RuleSet("SignupValidator", () =>
                {
                    validator.RuleFor(x => x.FirstName).NotNull().NotEmpty();
                    validator.RuleFor(x => x.LastName).NotNull().NotEmpty();
                    validator.RuleFor(x => x.Email).NotNull().NotEmpty();
                    validator.RuleFor(x => x.Password).NotNull().NotEmpty();

                });
                var valRes = validator.Validate(req, ruleSet: "SignupValidator");

                if (!valRes.IsValid)
                    return res.PrepareInvalidRequest(ref valRes);
                var data = userService.Signup(req,0);
                data.Token = TokenService.GenerateToken(data.Id);
                res.Data = data;
            }
            catch (Exception ex)
            {
                if (ex.Message ==
                    $"Duplicate entry '{req.Email}' for key 'UK_Users_Email'"
                )
                {
                    res.Message = $"The Email {req.Email} is already taken.";
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
        /// Login by username and password from db
        /// </summary>
        /// <returns></returns>


        [HttpPost("Login")]
        public ApiResponse Login(LoginRequest login)
        {
            var res = new ApiResponse();
            try
            {
                var password = Common.GetHash(login.Password); // Encrypt password
                var data = Common.Instances.UserInst.Login(login.Email, password);
                if (data.Id <= 0)
                {
                    res.Message = Constants.ResponseMessage.InvalidCredentials;
                    res.StatusCode = StatusCodes.Status404NotFound;
                    return res;
                }
                data.Token = TokenService.GenerateToken(data.Id);
                Common.FixNull(ref data);
                if (!data.IsActive)
                {
                    res.Message = !data.IsActive ? Constants.ResponseMessage.UserBlocked : Constants.ResponseMessage.ErrorEmailVerify;
                    res.StatusCode = StatusCodes.Status400BadRequest;
                    return res;
                }

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
        /// Add to friend list in db
        /// </summary>
        /// <returns></returns>

        [HttpPost("AddFriend")]
        public ApiResponse AddFriend(FriendRequest  req)
        {
            var res = new ApiResponse();
            try
            {
                var me = TokenService.GetUserByToken(Request.Headers);
                var validator = new InlineValidator<FriendRequest>();
                validator.RuleSet("FriendValidator", () =>
                {
                    validator.RuleFor(x => x.FirstName).NotNull().NotEmpty();
                    validator.RuleFor(x => x.LastName).NotNull().NotEmpty();
                    validator.RuleFor(x => x.Email).NotNull().NotEmpty();
                    validator.RuleFor(x => x.PhoneNumber).NotNull().NotEmpty();
                });
                var valRes = validator.Validate(req, ruleSet: "FriendValidator");

                if (!valRes.IsValid)
                    return res.PrepareInvalidRequest(ref valRes);
                res.Data = new FriendService().AddFriend(req, me.Id);
            }
            catch (Exception ex)
            {
                HttpContext.RiseError(ex);
                res.PrepareErrorResponse(ex);
            }
            return res;
        }

        /// <summary>
        /// Add trees planted by the user in db
        /// </summary>
        /// <returns></returns>

        [HttpPost("AddTreesPlanted")]
        public ApiResponse AddTreesPlanted()
        {
            var res = new ApiResponse();
            try
            {
                var me = TokenService.GetUserByToken(Request.Headers);
                res.Data = userService.AddTreesPlanted(me.Id);
            }
            catch (Exception ex)
            {
                HttpContext.RiseError(ex);
                res.PrepareErrorResponse(ex);
            }
            return res;
        }

        /// <summary>
        /// Delete user deatils by Id from db
        /// </summary>
        /// <returns></returns>

        [HttpDelete("Delete")]
        public ApiResponse Delete([Required] int id)
        {
            var res = new ApiResponse();
            try
            {
                var me = TokenService.GetUserByToken(Request.Headers);
                if (me.Id != id)
                    throw new UnauthorizedAccessException();

                res.Data = Common.Instances.UserInst.Delete(id);
            }
            catch (Exception ex)
            {
                HttpContext.RiseError(ex);
                res.PrepareErrorResponse(ex);
            }
            return res;
        }

        /// <summary>
        /// Save user deatils from db
        /// </summary>
        /// <returns></returns>

        [HttpPost("Save")]
        public ApiResponse Save(UserRequest userRequest)
        {
            var res = new ApiResponse();
            try
            {

                var me = TokenService.GetUserByToken(Request.Headers);
                var validator = new InlineValidator<UserRequest>();
                validator.RuleSet("UserRequestValidator", () =>
                {
                    validator.RuleFor(x => x.FirstName).NotNull().NotEmpty();
                    validator.RuleFor(x => x.LastName).NotNull().NotEmpty();
                    validator.RuleFor(x => x.HomeAddress).NotNull().NotEmpty();
                    validator.RuleFor(x => x.OfficeAddress).NotNull().NotEmpty();
                    validator.RuleFor(x => x.Email).NotNull().NotEmpty();
                    validator.RuleFor(x => x.Password).NotNull().NotEmpty();
                   
                });
                var valRes = validator.Validate(userRequest, ruleSet: "UserRequestValidator");
                if (!valRes.IsValid)
                    return res.PrepareInvalidRequest(ref valRes);

                var data = userService.SaveUser(userRequest, me.Id);
                data.Token = TokenService.GenerateToken(data.Id);
                res.Data = data;
            }
            catch (Exception ex)
            {
                if (ex.Message ==
                    $"Duplicate entry '{userRequest.Email}' for key 'UK_Users_Email'"
                )
                {
                    res.Message = $"The Email {userRequest.Email} is already taken.";
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


    }
}