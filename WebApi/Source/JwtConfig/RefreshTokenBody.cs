using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FRS.WebApi.JwtConfig
{
    /// <summary>
    /// 用来刷新token的请求body
    /// </summary>
    public class RefreshTokenBody
    {
        private string accessToken = string.Empty;

        private string refreshToken = string.Empty;

        /// <summary>
        /// 原始签发的token
        /// </summary>
        public string AccessToken { get => accessToken; set => accessToken = value; }

        /// <summary>
        /// 刷新token时的二次验证token,
        /// 该token在登录的时候返回，过期时间较长，一旦拥有后，过期之后不再支持token重新签发，必须重新登录
        /// </summary>
        public string RefreshToken { get => refreshToken; set => refreshToken = value; }


    }
}
