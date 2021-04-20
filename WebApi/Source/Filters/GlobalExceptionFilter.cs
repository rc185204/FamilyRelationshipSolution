using FRS.Common;
using FRS.WebApi.Log;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FRS.WebApi.Filters
{
    /// <summary>
    /// 异常处理过滤器
    /// </summary>
    public class GlobalExceptionFilter : Attribute, IExceptionFilter
    {
        private readonly IHostEnvironment _hostingEnvironment;
        private readonly IModelMetadataProvider _modelMetadataProvider;
        private readonly ILoggerHelper _loggerHelper;

        /// <summary>
        /// 构造异常过滤器
        /// </summary>
        /// <param name="loggerHelper"></param>
        /// <param name="hostingEnvironment"></param>
        /// <param name="modelMetadataProvider"></param>
        public GlobalExceptionFilter(ILoggerHelper loggerHelper, IHostEnvironment hostingEnvironment, IModelMetadataProvider modelMetadataProvider)
        {
            _loggerHelper = loggerHelper;
            _hostingEnvironment = hostingEnvironment;
            _modelMetadataProvider = modelMetadataProvider;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void OnException(ExceptionContext context)
        {
            _loggerHelper.Error(typeof(GlobalExceptionFilter), "OnException", context.Exception);
            ContentResult result = new ContentResult
            {
                StatusCode = 500,
                ContentType = "text/json;charset=utf-8;"
            };

            // 自定义返回的错误信息
            if (_hostingEnvironment.IsDevelopment())
            {
                HttpResponse response = new HttpResponse(ErrorCode.Unknown_Error, null, context.Exception.StackTrace);
                result.Content = JsonConvert.SerializeObject(response);
            }
            else
            {
                HttpResponse response = new HttpResponse(ErrorCode.Unknown_Error, null, "Unknown exception error, please connect admin");
                result.Content = JsonConvert.SerializeObject(response);
            }
            context.Result = result;
            context.ExceptionHandled = true;
        }
    }
}
