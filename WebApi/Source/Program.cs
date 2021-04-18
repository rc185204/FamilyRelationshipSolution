using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FRS.WebApi
{
    /// <summary>
    /// 
    /// </summary>
    public class Program
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    string path = Directory.GetCurrentDirectory();
                    Console.WriteLine("path:" + path);
                    var builder = new ConfigurationBuilder()
                    .SetBasePath(path)
                    .AddJsonFile("appsettings.json");
                    var config = builder.Build();
                    string usrl = "http://*:4098;https://*:4099";
                    var AppSettings = config.GetSection("AppSettings");
                    if (AppSettings != null)
                    {
                        string url = AppSettings.GetSection("UseUrls").Value;
                        if (!string.IsNullOrEmpty(url))
                            usrl = url;
                        else
                            Console.WriteLine("read url empty");
                    }
                    Console.WriteLine(usrl); // 配置键
                    // string usrl = "http://*:4098;https://*:4099";// Common.AppSettings.GetAppSeting("UseUrls");
                    webBuilder.UseUrls(usrl);// 指定开启的端口
                });
    }
}
