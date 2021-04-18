using FRS.BusinessLayer;
using FRS.Common;
using FRS.Models;
using FRS.WebApi.Filters;
using FRS.WebApi.Helper;
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
    [Authorize]
    [Route("api/[controller]")]
    [ApiVersion("1.0", Deprecated = true)]
    public class UserController : ControllerBase
    {

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="obj">POST请求的表单数据放在 body中，而GET的请求参数在url中，需要使用FromUri</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<HttpResponse> Post([FromBody] User obj)
        {
            ErrorCode rev = await Task.Factory.StartNew<ErrorCode>(() => BLUser.Add(obj));
            HttpResponse response = new HttpResponse(rev, null);
            return response;
        }
    }
}
