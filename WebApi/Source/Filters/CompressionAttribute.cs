using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FRS.WebApi.Filters
{
    /// <summary>
    /// 压缩过滤器，建议使用在返回数据比较大的action中，因为压缩本身需要消耗时间，如果较小的数据进行压缩，效果不明显，反而会消耗时间，从而降低执行效率
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class CompressionAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// 对执行后的返回结果进行压缩
        /// </summary>
        /// <param name="actContext"></param>
        public override void OnActionExecuted(ActionExecutedContext actContext)
        {
            string acceptEncoding = actContext.HttpContext.Request.Headers["Accept-Encoding"];
            if (acceptEncoding.Contains("deflate"))
            {
                var stream = actContext.HttpContext.Response.Body;
                System.IO.Compression.DeflateStream compressStrem = new System.IO.Compression.DeflateStream(stream, System.IO.Compression.CompressionMode.Compress);
                actContext.HttpContext.Response.Body = compressStrem;
                actContext.HttpContext.Response.Headers.Add("Content-encoding", "deflate");
            }
            else if (acceptEncoding.Contains("gzip"))
            {
                var stream = actContext.HttpContext.Response.Body;
                System.IO.Compression.GZipStream compressStrem = new System.IO.Compression.GZipStream(stream, System.IO.Compression.CompressionMode.Compress);
                actContext.HttpContext.Response.Body = compressStrem;
                actContext.HttpContext.Response.Headers.Add("Content-encoding", "gzip");
            }
            base.OnActionExecuted(actContext);
        }
    }
}
