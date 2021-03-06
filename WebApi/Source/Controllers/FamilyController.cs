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
    /// 
    /// </summary>
    [ApiController]
    [ServiceFilter(typeof(GlobalExceptionFilter))]
    [Authorize]
    [Route("api/[controller]")]
    [ApiVersion("1.0", Deprecated = true)]
    public class FamilyController : ControllerBase
    {
        /// <summary>
        /// 查找Family信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "System admin")]
        public async Task<HttpResponse> Get()
        {
            ErrorCode rev = ErrorCode.Unknown_Error;
            List<Family> list = await Task.Factory.StartNew(() => BLFamily.GetAll());
            if (list == null)
                rev = ErrorCode.DataNotExist;
            else
                rev = ErrorCode.Success;
            HttpResponse response = new(rev, list);
            return response;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPut]
        [Authorize(Roles = "System admin, Family admin")]
        public async Task<HttpResponse> Put([FromBody] Family obj)
        {
            ErrorCode rev = await Task.Factory.StartNew(() => BLFamily.Add(obj));
            HttpResponse response = new(rev, null);
            return response;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpDelete]
        [Authorize(Roles = "System admin, Family admin")]
        public async Task<HttpResponse> Delete([FromBody] Family obj)
        {
            ErrorCode rev = await Task.Factory.StartNew(() => BLFamily.Remove(obj));
            HttpResponse response = new(rev, null);
            return response;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "System admin, Family admin")]
        public async Task<HttpResponse> Post([FromBody] Family obj)
        {
            ErrorCode rev = await Task.Factory.StartNew(() => BLFamily.Modify(obj));
            HttpResponse response = new(rev, null);
            return response;
        }
    }
}
