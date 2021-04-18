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
    public class FileUploadOperation : IOperationFilter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="context"></param>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            //判断上传文件的类型，只有上传的类型是IFormCollection的才进行重写。
            if (context.ApiDescription.ActionDescriptor.Parameters.Any(w => w.ParameterType == typeof(Microsoft.AspNetCore.Http.IFormCollection)))
            {
                Dictionary<string, OpenApiSchema> schema = new Dictionary<string, OpenApiSchema>();
                schema["fileName"] = new OpenApiSchema { Description = "Select file", Type = "string", Format = "binary" };
                Dictionary<string, OpenApiMediaType> content = new Dictionary<string, OpenApiMediaType>();
                content["multipart/form-data"] = new OpenApiMediaType { Schema = new OpenApiSchema { Type = "object", Properties = schema } };
                operation.RequestBody = new OpenApiRequestBody() { Content = content };
            }
        }
    }
}
