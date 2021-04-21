using FRS.BusinessLayer;
using FRS.Common;
using FRS.Models;
using FRS.WebApi.Filters;
using FRS.WebApi.JwtConfig;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FRS.WebApi.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [ServiceFilter(typeof(GlobalExceptionFilter))]
    //[Authorize]
    [Route("api/[controller]")]
    [ApiVersion("1.0", Deprecated = true)]
    public class UserController : ControllerBase
    {

        /// <summary>
        /// 模糊查询
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="UserTrueName"></param>
        /// <param name="RoleId"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "System admin, Family admin")]
        public async Task<HttpResponse> Get([FromQuery] string UserName, string UserTrueName, int? RoleId)
        {
            User user = new();
            user.UserName = UserName;
            user.UserTrueName = UserTrueName;
            user.RoleId = RoleId == null ? 0 : RoleId.Value;
            ErrorCode rev = ErrorCode.Unknown_Error;
            List<User> list = await Task.Factory.StartNew(() => BLUser.InquiryUsers(user));
            if (list == null || list.Count == 0)
                rev = ErrorCode.DataNotExist;
            else
                rev = ErrorCode.Success;
            HttpResponse response = new(rev, list);
            return response;
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="obj">POST请求的表单数据放在 body中，而GET的请求参数在url中，需要使用FromUri</param>
        /// <returns></returns>
        [HttpPost("Add User")]
        [Authorize(Roles = "System admin, Family admin")]
        public async Task<HttpResponse> Post([FromBody] User obj)
        {
            string accessToken = HttpContext.Request.Headers["Authorization"];
            User adminUser = await Task.Factory.StartNew(() => GetUser(accessToken));
            ErrorCode rev = await Task.Factory.StartNew(() => BLUser.Add(obj, adminUser));
            HttpResponse response = new (rev, null);
            return response;
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpDelete("Delete User")]
        [Authorize(Roles = "System admin, Family admin")]
        public async Task<HttpResponse> Delete([FromBody] User obj)
        {
            string accessToken = HttpContext.Request.Headers["Authorization"];
            User adminUser = await Task.Factory.StartNew(() => GetUser(accessToken));
            ErrorCode rev = await Task.Factory.StartNew(() => BLUser.Remove(obj, adminUser));
            HttpResponse response = new(rev, null);
            return response;
        }

        /// <summary>
        /// 修改用户密码,TrueName
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPut]
        [Authorize]
        public async Task<HttpResponse> Put(string userName, string password, User obj)
        {
            ErrorCode rev = ErrorCode.Unknown_Error;
            User user = await Task.Factory.StartNew(() => BLUser.Valid(userName, password));
            if (user == null)
                rev = ErrorCode.No_User;
            else
                rev = await Task.Factory.StartNew(() => BLUser.ModifyBaseInfo(obj));
            HttpResponse response = new(rev, null);
            return response;
        }

        private static User GetUser(string accessToken)
        {
            string userName = GenerateJwt.GetAccessUser(accessToken.Remove(0, 7));
            User adminUser = BLUser.Get(userName);
            return adminUser;
        }
    }
}
