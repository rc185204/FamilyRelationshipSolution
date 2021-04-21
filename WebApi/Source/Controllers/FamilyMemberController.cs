using FRS.BusinessLayer;
using FRS.Common;
using FRS.Models;
using FRS.WebApi.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FRS.WebApi.Controllers
{
    /// <summary>
    /// 家族成员控制器
    /// 本家族用户可以查看所有家族成员信息，
    /// member成员所有数据，包括自己的数据，应该由家庭管理员修改
    /// </summary>
    [ApiController]
    [ServiceFilter(typeof(GlobalExceptionFilter))]
    [Authorize]
    [Route("api/[controller]")]
    [ApiVersion("1.0", Deprecated = true)]
    public class FamilyMemberController : ControllerBase
    {
        /// <summary>
        /// 获取家庭成员列表
        /// </summary>
        /// <param name="FamilyId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<HttpResponse> Get([FromQuery] string FamilyId)
        {            
            ErrorCode rev = ErrorCode.Unknown_Error;
            List<FamilyMember> list = await Task.Factory.StartNew(() => BLFamilyMember.GetMembers(FamilyId));
            if (list == null || list.Count == 0)
                rev = ErrorCode.DataNotExist;
            else
                rev = ErrorCode.Success;
            HttpResponse response = new(rev, list);
            return response;
        }

        /// <summary>
        /// 添加家族成员
        /// </summary>
        /// <param name="obj">POST请求的表单数据放在 body中，而GET的请求参数在url中，需要使用FromUri</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "System admin, Family admin")]
        public async Task<HttpResponse> Post([FromBody] FamilyMember obj)
        {
            ErrorCode rev = await Task.Factory.StartNew(() => BLFamilyMember.Add(obj));
            HttpResponse response = new(rev, null);
            return response;
        }

        /// <summary>
        /// 删除成员
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpDelete]
        [Authorize(Roles = "System admin, Family admin")]
        public async Task<HttpResponse> Delete([FromBody] FamilyMember obj)
        {
            ErrorCode rev = await Task.Factory.StartNew(() => BLFamilyMember.Remove(obj));
            HttpResponse response = new(rev, null);
            return response;
        }

        /// <summary>
        /// 修改成员信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPut]
        [Authorize(Roles = "System admin, Family admin")]
        public async Task<HttpResponse> Put([FromBody] FamilyMember obj)
        {
            ErrorCode rev = await Task.Factory.StartNew(() => BLFamilyMember.Modify(obj));
            HttpResponse response = new(rev, null);
            return response;
        }

    }
}
