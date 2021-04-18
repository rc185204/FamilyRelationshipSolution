using FRS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FRS.WebApi.JwtConfig
{
    /// <summary>
    /// 
    /// </summary>
    public class JwtTokenResult
    {
        /// <summary>
        /// 
        /// </summary>
        public string access_token { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string refresh_token { get; set; }

        /// <summary>
        /// 过期时间(单位秒)
        /// </summary>
        public int expires_in { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string token_type { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public User user { get; set; }
    }
}
