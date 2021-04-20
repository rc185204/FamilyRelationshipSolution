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
    //[Authorize]
    [Route("api/[controller]")]
    public class CertificateTypeController : ControllerBase
    {
        /// <summary>
        /// 添加证件类型
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<HttpResponse> Post([FromBody] CertificateType obj)
        {
            ErrorCode rev = await Task.Factory.StartNew<ErrorCode>(() => BLCertificateType.Add(obj));
            HttpResponse response = new HttpResponse(rev, null);
            return response;
        }

        /// <summary>
        /// 删除证件类型
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<HttpResponse> Delete([FromBody] CertificateType obj)
        {
            ErrorCode rev = await Task.Factory.StartNew<ErrorCode>(() => BLCertificateType.Remove(obj));
            HttpResponse response = new HttpResponse(rev, null);
            return response;
        }

        /// <summary>
        /// 修改证件类型信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPut("Modify CertificateType")]
        public async Task<HttpResponse> Put([FromBody] CertificateType obj)
        {
            ErrorCode rev = await Task.Factory.StartNew<ErrorCode>(() => BLCertificateType.Modify(obj));
            HttpResponse response = new HttpResponse(rev, null);
            return response;
        }
    }
}
