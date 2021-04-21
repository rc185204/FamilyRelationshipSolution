using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FRS.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class AppSettings
    {
        private static IConfigurationSection appSection = null;

        public static string SqlConn;

        /// <summary>
        /// set config section
        /// </summary>
        /// <param name="section"></param>
        public static void SetAppSetting(IConfigurationSection section)
        {
            appSection = section;
        }

        /// <summary>
        /// get the config string form current section by the key
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
