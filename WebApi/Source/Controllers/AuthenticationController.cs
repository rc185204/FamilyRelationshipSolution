using FRS.WebApi.Filters;
using FRS.WebApi.JwtConfig;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using FRS.BusinessLayer;
using FRS.Models;
using FRS.WebApi.Log;
using FRS.Common;

namespace FRS.WebApi.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [ServiceFilter(typeof(CompressionAttribute))]
    [ServiceFilter(typeof(GlobalExceptionFilter))]
    //[EnableCors("any")] // startup.cs中已经加入了跨域处理了，app.UseCors("any");这里不需要再申明
    [Route("api/[controller]")]
    [ApiVersion("1.0", Deprecated = true)]//当哪个Api版本不在更新,就需要弃用掉这个版本。Deprecated=true说明该Api版本已经弃用,但是弃用不代表不能请求。只是会在响应头中告知次版本已经已经弃用。
    [ApiVersion("2.0")]

    public class AuthenticationController : ControllerBase
    {
        private readonly ILoggerHelper _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loggerHelper"></param>
        public AuthenticationController(ILoggerHelper loggerHelper)
        {
            _logger = loggerHelper;
        }

        /// <summary>
        /// 异步用户认证
        /// </summary>
        /// <param name="uname">User name</param>
        /// <param name="pwd">Password</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public async Task<HttpResponse> Get(string uname, string pwd)
        {
            //_logger.Error(typeof(AuthenticationController), "这是错误日志", new Exception("123"));
            //_logger.Debug(typeof(AuthenticationController), "这个是bug日志");
            //throw new Exception("error");

            string jwtStr = string.Empty;
            string roleName = string.Empty;
            User user = null;
            bool suc = false;
            if (ModelState.IsValid)
            {
                user = await Task.Factory.StartNew(() => BLUser.Valid(uname, pwd));
                if (user != null)
                {
                    //var refreshToken = Guid.NewGuid().ToString();
                    var jwtTokenResult = GenerateJwt.GenerateEncodedTokenAsync(user);
                    jwtStr = jwtTokenResult;//登录，获取到一定规则的 Token 令牌
                    suc = true;
                }
            }
            ErrorCode errorCode = suc ? ErrorCode.Success : ErrorCode.Login_Error;
            string description = suc ? string.Empty : "user name or password error";

            HttpResponse response = new (
                errorCode,
                Ok(value: new { token = jwtStr, user = user }),
                description);
            return response;
        }

        /// <summary>
        /// 刷新身份认证
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("RefreshToken")]
        public async Task<HttpResponse> Post([FromBody] RefreshTokenBody request)
        {
            string jwtStr = string.Empty;
            string description = string.Empty;
            ClaimsPrincipal simplePrinciple = null;
            bool suc = await Task.Factory.StartNew(() => GenerateJwt.ValidateAccessToken(request.AccessToken, out simplePrinciple));
            ErrorCode errorCode = suc ? ErrorCode.Success : ErrorCode.AccessToken_NoUseful;
            if (suc)
                jwtStr = GenerateJwt.RefreshEncodedTokenAsync(simplePrinciple);
            else
                description = "AccessToken has expired";

            HttpResponse response = new (
                errorCode,
                Ok(value: new { token = jwtStr }),
                description
                );
            return response;
        }


    }
}
