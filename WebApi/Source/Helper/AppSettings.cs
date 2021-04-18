using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FRS.WebApi.Helper
{
    /// <summary>
    /// 
    /// </summary>
    public class AppSettings
    {
        private static IConfigurationSection appSection = null;

        /// <summary>
        /// 设置配置文件
        /// </summary>
        /// <param name="section"></param>
        public static void SetAppSetting(IConfigurationSection section)
        {
            appSection = section;
        }

        /// <summary>
        /// 获取配置文件
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetAppSeting(string key)
        {
            if (appSection.GetSection(key) != null)
            {
                return appSection.GetSection(key).Value;
            }
            else
            {
                return "";
            }
        }
    }
}
