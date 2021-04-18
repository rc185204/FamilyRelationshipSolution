using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FRS.WebApi.Filters
{
    /// <summary>
    /// 
    /// </summary>
    public class ApiVersionHeaderParameter : IOperationFilter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="context"></param>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // 获取该Action 是否有[Authorize]过滤器，如果有，则认为需要token认证
            var isNeedApiVersion = context.MethodInfo.DeclaringType.GetCustomAttributes(true).OfType<ApiVersionAttribute>().Any() ||
                        context.MethodInfo.GetCustomAttributes(true).OfType<ApiVersionAttribute>().Any();
            if (!isNeedApiVersion)
                return;
            if (operation.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>();

            // 添加自定义参数名为 Token 的数据 放入 header中 
            operation.Parameters.Add(new OpenApiParameter { Name = "api-version", In = ParameterLocation.Header, Description = "api版本控制", Required = false, });

        }
    }
}
