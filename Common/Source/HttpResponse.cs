using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FRS.Common
{
    public class HttpResponse
    {
        private bool success;

        private ErrorCode errorCode;

        private string description;

        private object jsonData;

        /// <summary>
        /// 
        /// </summary>
        public bool Success { get { return success = ((ErrorCode == ErrorCode.Success) ? true : false); } }

        /// <summary>
        /// 
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public ErrorCode ErrorCode { get => errorCode; set => errorCode = value; }

        /// <summary>
        /// 
        /// </summary>
        public string Description { get => description; set => description = value; }

        /// <summary>
        /// 
        /// </summary>
        public object JsonData { get => jsonData; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ErrorCode"></param>
        /// <param name="JsonData"></param>
        /// <param name="description"></param>
        public HttpResponse(ErrorCode ErrorCode, object JsonData, string description = null)
        {
            this.ErrorCode = ErrorCode;
            this.jsonData = JsonData;
            this.description = description;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="JsonData"></param>
        public HttpResponse(object JsonData)
        {
            this.jsonData = JsonData;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

}
