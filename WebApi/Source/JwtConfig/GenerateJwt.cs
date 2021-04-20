using FRS.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FRS.WebApi.JwtConfig
{
    /// <summary>
    /// 
    /// </summary>
    public static class GenerateJwt
    {
        private static JwtConfig _jwtConfig;

        /// <summary>
        /// 设置配置文件
        /// </summary>
        /// <param name="section"></param>
        public static void SetJwtConfig(JwtConfig section)
        {
            _jwtConfig = section;
        }

        /// <summary>
        /// 生成token
        /// </summary>
        /// <param name="customClaims">携带的用户信息</param>
        /// <returns></returns>
        public static string GenerateEncodedTokenAsync(User customClaims)
        {
            //创建用户身份标识，可按需要添加更多信息
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.SerialNumber, customClaims.UserId.ToString()),
                new Claim(ClaimTypes.Name, customClaims.UserName),
                new Claim(ClaimTypes.Role, customClaims.Role.RoleName),
                new Claim("realname", customClaims.UserTrueName),
                new Claim(JwtRegisteredClaimNames.Nbf,$"{new DateTimeOffset(_jwtConfig.NotBefore).ToUnixTimeSeconds()}") ,
                new Claim(JwtRegisteredClaimNames.Exp,$"{new DateTimeOffset(_jwtConfig.Expiration).ToUnixTimeSeconds()}"),
            };
            // 可以将一个用户的多个角色全部赋予
            claims.AddRange(customClaims.Role.RoleName.Split(',').Select(s => new Claim(ClaimTypes.Role, s)));

            //创建令牌
            var jwt = new JwtSecurityToken(
                issuer: _jwtConfig.Issuer,
                audience: _jwtConfig.Audience,
                claims: claims,
                notBefore: _jwtConfig.NotBefore,
                expires: _jwtConfig.Expiration,
                signingCredentials: _jwtConfig.SigningCredentials);

            string access_token = new JwtSecurityTokenHandler().WriteToken(jwt);
            return access_token;
        }

        /// <summary>
        /// 刷新token信息
        /// </summary>
        /// <param name="simplePrinciple"></param>
        /// <returns></returns>
        public static string RefreshEncodedTokenAsync(ClaimsPrincipal simplePrinciple)
        {
            string access_token = string.Empty;

            if (simplePrinciple != null)
            {
                var claims = new List<Claim>();
                foreach (var claim in simplePrinciple.Claims)
                {
                    switch (claim.Type)
                    {
                        case JwtRegisteredClaimNames.Nbf:
                            claims.Add(new Claim(claim.Type, $"{new DateTimeOffset(_jwtConfig.NotBefore).ToUnixTimeSeconds()}"));
                            break;
                        case JwtRegisteredClaimNames.Exp:
                            claims.Add(new Claim(claim.Type, $"{new DateTimeOffset(_jwtConfig.Expiration).ToUnixTimeSeconds()}"));
                            break;
                        default:
                            claims.Add(new Claim(claim.Type, claim.Value));
                            break;
                    }
                }
                var jwt = new JwtSecurityToken(
                    issuer: _jwtConfig.Issuer,
                    audience: _jwtConfig.Audience,
                    claims: simplePrinciple.Claims,
                    notBefore: _jwtConfig.NotBefore,
                    expires: _jwtConfig.Expiration,
                    signingCredentials: _jwtConfig.SigningCredentials);
                access_token = new JwtSecurityTokenHandler().WriteToken(jwt);
            }

            return access_token;
        }

        /// <summary>
        /// 解析token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static ClaimsPrincipal SerializeJwt(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();

                if (tokenHandler.ReadToken(token) is not JwtSecurityToken jwtToken)
                    return null;

                //var symmetricKey = Convert.FromBase64String(_jwtConfig.SecretKey);

                var validationParameters = new TokenValidationParameters()
                {
                    ValidIssuer = _jwtConfig.Issuer,
                    //颁发给谁
                    ValidAudience = _jwtConfig.Audience,
                    RequireExpirationTime = false,// 这里暂时不去判断时间是否过期，只判断是否正确的token，为了当token在允许过期后的某个时间段内刷新token
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.SecretKey)),
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out _);
                return principal;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 验证客户端的AccessToken
        /// </summary>
        /// <param name="token"></param>
        /// <param name="simplePrinciple"></param>
        /// <returns></returns>
        public static bool ValidateAccessToken(string token, out ClaimsPrincipal simplePrinciple)
        {
            simplePrinciple = SerializeJwt(token);
            //ClaimsIdentity identity = simplePrinciple?.Identity as ClaimsIdentity;
            //if (identity == null)
            //    return false;

            if (simplePrinciple?.Identity is not ClaimsIdentity identity)
                return false;
            if (!identity.IsAuthenticated)
                return false;

            var usernameClaim = identity.FindFirst(ClaimTypes.Name);
            string username = usernameClaim?.Value;

            if (string.IsNullOrEmpty(username))
                return false;

            var expirationClaim = identity.FindFirst(JwtRegisteredClaimNames.Exp);
            var expiration = expirationClaim?.Value;
            _ = long.TryParse(expiration, out long exporationSeconds);
            var UnixTimeSecondsNow = new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds();

            // 判断AccessToken是否过期，如果原始token没有过期，不签发新证书
            if (exporationSeconds > UnixTimeSecondsNow)
                return false;

            // 需要判断，刷新token的时候，是否超过了限时时间
            if (exporationSeconds + _jwtConfig.RefreshTokenExpired < UnixTimeSecondsNow)
                return false;
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static string GetAccessUser(string token)
        {
            string userName = string.Empty;
            ClaimsPrincipal simplePrinciple = SerializeJwt(token);
            if (simplePrinciple?.Identity is not ClaimsIdentity identity)
                return userName;
            if (!identity.IsAuthenticated)
                return userName;

            var usernameClaim = identity.FindFirst(ClaimTypes.Name);
            userName = usernameClaim?.Value;            
            return userName;

            
        }
    }

}
