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
        /// 查找对应FamilyId的家族信息, 实际使用中，由于用户对象已经关联外键 Family，因此用户自身已经知道本Family信息，这里不需要再额外查询
        /// </summary>
        /// <param name="FamilyId"></param>
        /// <returns></returns>
        //[HttpGet]
        //public async Task<HttpResponse> Get([FromQuery] string FamilyId)
        //{            
        //    ErrorCode rev = ErrorCode.Unknown_Error;
        //    Family Family = await Task.Factory.StartNew(() => BLFamily.Get(FamilyId));
        //    if (Family == null)
        //        rev = ErrorCode.DataNotExist;
        //    else
        //        rev = ErrorCode.Success;
        //    HttpResponse response = new(rev, Family);
        //    return response;
        //}

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
    }
}
