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
    /// 证件类型控制器
    /// </summary>
    [ApiController]
    [ServiceFilter(typeof(GlobalExceptionFilter))]
    [Authorize]
    [Route("api/[controller]")]
    public class CertificateTypeController : ControllerBase
    {
        /// <summary>
        /// 获取所有证件类型集合
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<HttpResponse> Get()
        {
            ErrorCode rev = ErrorCode.Unknown_Error;
            List <CertificateType> list = await Task.Factory.StartNew(() => BLCertificateType.GetAll());
            if (list == null || list.Count == 0)
                rev = ErrorCode.DataNotExist;
            else
                rev = ErrorCode.Success;
            HttpResponse response = new (rev, list);
            return response;
        }

        /// <summary>
        /// 添加证件类型
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "System admin")]
        public async Task<HttpResponse> Post([FromBody] CertificateType obj)
        {
            ErrorCode rev = await Task.Factory.StartNew(() => BLCertificateType.Add(obj));
            HttpResponse response = new (rev, null);
            return response;
        }

        /// <summary>
        /// 删除证件类型
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpDelete]
        [Authorize(Roles = "System admin")]
        public async Task<HttpResponse> Delete([FromBody] CertificateType obj)
        {
            ErrorCode rev = await Task.Factory.StartNew(() => BLCertificateType.Remove(obj));
            HttpResponse response = new (rev, null);
            return response;
        }

        /// <summary>
        /// 修改证件类型信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPut("Modify CertificateType")]
        [Authorize(Roles = "System admin")]
        public async Task<HttpResponse> Put([FromBody] CertificateType obj)
        {
            ErrorCode rev = await Task.Factory.StartNew(() => BLCertificateType.Modify(obj));
            HttpResponse response = new (rev, null);
            return response;
        }
    }
}
