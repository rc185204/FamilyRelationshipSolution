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
    }
}
