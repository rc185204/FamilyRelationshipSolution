using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FRS.WebApi.JwtConfig
{
    /// <summary>
    /// 
    /// </summary>
    public class JwtConfig : IOptions<JwtConfig>
    {
        /// <summary>
        /// 
        /// </summary>
        public JwtConfig Value => this;

        /// <summary>
        /// 
        /// </summary>
        public string SecretKey { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Expired { get; set; }

        /// <summary>
        /// 刷新token的时间与token的失效时间间隔m
        /// 当token的失效后，需要刷新token的时候，
        /// 在ValidateAccessToken时，需要判断token的失效时间是否超过当前值，如果超过了当前值，则不再支持刷新
        /// </summary>
        public int RefreshTokenExpired { get; set; }

        /// <summary>
        /// 使用国际标准时间
        /// </summary>
        public DateTime NotBefore => DateTime.UtcNow;

        /// <summary>
        /// 
        /// </summary>
        public DateTime IssuedAt => DateTime.UtcNow;

        /// <summary>
        /// 
        /// </summary>
        public DateTime Expiration => IssuedAt.AddMinutes(Expired);

        /// <summary>
        /// 
        /// </summary>
        public SecurityKey SigningKey => new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));

        /// <summary>
        /// 
        /// </summary>
        public SigningCredentials SigningCredentials =>
            new(SigningKey, SecurityAlgorithms.HmacSha256);
    }
}
