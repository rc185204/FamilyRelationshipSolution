using FRS.DatabaseContext;
using FRS.WebApi.Filters;
using FRS.WebApi.Helper;
using FRS.WebApi.JwtConfig;
using FRS.WebApi.Log;
using log4net;
using log4net.Config;
using log4net.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FRS.WebApi
{
    /// <summary>
    /// 
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// log4net 仓储库
        /// </summary>
        public static ILoggerRepository repository { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// 
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            AppSettings.SetAppSetting(Configuration.GetSection("AppSettings"));

            //services.AddDbContext<FamilyRelationshipContext>(options =>
            //options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            #region Swagger配置

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "FRS.WebApi", Version = "v1" });
                c.SwaggerDoc("v2", new OpenApiInfo { Title = "FRS.WebApi", Version = "v2" });// API 分组
                // 当一个controller中出现多个[httpPost]/[httpGet]时，默认使用第一个
                // 正确的情况是一个controller中只能包含一个get/post/put/delete,尽管多个get出现的时候是可以的，但是Swagger并不支持这种做法，
                // 而是建议将多个action 拆分成多个controller
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

                // 显示导出函数<summary>说明
                // 1.生成属性中生成xml
                // 2.c.IncludeXmlComments
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

                //Swagger增加自定义输入UI
                //c.OperationFilter<AuthTokenHeaderParameter>();
                c.OperationFilter<ApiVersionHeaderParameter>();

                //在UI加入授权认证按钮
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "授权认证, 输入Bearer {token}(注意两者之间是一个空格) \"",
                    Name = "Authorization",//jwt默认的参数名称
                    In = ParameterLocation.Header,//jwt默认存放Authorization信息的位置(请求头中)
                    Scheme = "Bearer",
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT"
                });

                //把所有方法配置为增加bearer头部信息
                //每个API方法后，会有一个锁的标志，表明该方法会传递bearer token。
                var securityRequirement = new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                };

                c.AddSecurityRequirement(securityRequirement);
                // 给Swagger配置FileUploadOperation
                c.OperationFilter<FileUploadOperation>();
            });

            #endregion

            #region 添加 jwt 认证
            services.Configure<JwtConfig.JwtConfig>(Configuration.GetSection("JwtConfig"));
            var jwtConfig = new JwtConfig.JwtConfig();
            Configuration.Bind("JwtConfig", jwtConfig);
            GenerateJwt.SetJwtConfig(jwtConfig);

            services.AddAuthentication(option =>
            {
                //认证middleware配置
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                //默认方案
                option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            })
               .AddJwtBearer(options =>
               {
                   options.TokenValidationParameters = new TokenValidationParameters
                   {
                       //注意这是缓冲过期时间，总的有效时间等于这个时间加上jwt的过期时间，如果不配置，默认是5分钟
                       ClockSkew = TimeSpan.FromSeconds(4),

                       //Token颁发机构
                       ValidIssuer = jwtConfig.Issuer,
                       //颁发给谁
                       ValidAudience = jwtConfig.Audience,
                       ValidateIssuer = true,
                       ValidateAudience = true,
                       ValidateLifetime = true, //是否验证Token有效期，使用当前时间与Token的Claims中的NotBefore和Expires对比
                                                //这里的key要进行加密
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.SecretKey)),
                   };
                   options.Events = new JwtBearerEvents
                   {
                       OnAuthenticationFailed = context =>
                       {
                           // 如果过期，则把添加到，返回头信息中
                           if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                           {
                               context.Response.Headers.Add("Token-Expired", "true");

                               // 必须加入Access-Control-Allow-* ，在有跨域认证的请求中加入，否则当token失效后，会返回跨域错误
                               context.Response.Headers.Add("Access-Control-Allow-Credentials", "true");
                               string Origin = context.Request.Headers["origin"];
                               context.Response.Headers.Add("Access-Control-Allow-Origin", Origin);

                               // 定义在token失效后的返回数据中，允许跨域请求读取的headers的标签
                               context.Response.Headers.Add("Access-Control-Allow-Headers", "Token-Expired");
                               context.Response.Headers.Add("Access-Control-Expose-Headers", "Token-Expired,Content-Type");
                           }
                           return Task.CompletedTask;
                       }
                   };
               });
            #endregion

            #region json字符配置
            //配置 .NET Core WebApi中返回 json 数据首字母大小写保留字段原始大小写， 不配置的情况下默认首字母小写
            services.AddControllers().AddJsonOptions(config =>
            {
                config.JsonSerializerOptions.PropertyNamingPolicy = null;
            });

            //在swagger中将枚举显示为字符串
            services.AddControllers().AddJsonOptions(options =>
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
            #endregion

            # region 日志注入
            // AddSingleton 项目启动-项目关闭.相当于静态类,只会有一个
            services.AddSingleton<ILoggerHelper, LoggerHelper>();

            //log4net
            repository = LogManager.CreateRepository("FRS.WebApi");//项目名称
            XmlConfigurator.Configure(repository, new FileInfo("log4net.config"));//指定配置文件
            #endregion

            #region 将异常过滤器注入到容器中
            // AddScoped : 请求开始-请求结束  在这次请求中获取的对象都是同一个。在请求发生的时候产生过滤器
            services.AddScoped<GlobalExceptionFilter>();
            #endregion

            #region 注入压缩过滤器
            services.AddScoped<CompressionAttribute>();
            #endregion

            #region 添加cors服务 配置跨域处理
            string[] originsarray = new string[] { "http://localhost:5000", "http://localhost:5001" };
            string origins = AppSettings.GetAppSeting("PolicyWithOrigins");
            if (!string.IsNullOrEmpty(origins))
            {
                originsarray = origins.Split(',');
            }
            Console.WriteLine("WithOrigins:" + string.Join(",", originsarray));
            services.AddCors(options =>
            {
                options.AddPolicy("any", builder =>
                {
                    builder
                    //.AllowAnyOrigin()
                    .WithOrigins(originsarray)//允许任何来源的主机访问
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();//指定处理cookie
                });
            });
            #endregion

            #region 版本控制
            services.AddApiVersioning(opt =>
            {
                opt.AssumeDefaultVersionWhenUnspecified = true;
                opt.DefaultApiVersion = new ApiVersion(1, 0);
                opt.ApiVersionReader = ApiVersionReader.Combine(
                    //new MediaTypeApiVersionReader("version")// 通过MediaType,当定义了2个同名 Controller 后，通过content-type中申明的版本号返回对应的controller,如果不申明版本号，则返回最新版本号对应的controller
                    //new QueryStringApiVersionReader(parameterNames:"version"),// 通过查询字符串控制版本：https://localhost:44392/api/Version?version=1.0
                    new HeaderApiVersionReader("api-version")//通过header申明版本号
                    );
                opt.ReportApiVersions = true;
            });

            #endregion
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "FRS.WebApi v1");
                    c.SwaggerEndpoint("/swagger/v2/swagger.json", "FRS.WebApi v2");
                });
            }
            else 
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();

                // 部署环境下开启Swagger，为了内部跨团队合作时候，其他团队成员在内部服务器访问调试
                // 实际部署生产环境需要拿掉
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "FRS.WebApi v1");
                    c.SwaggerEndpoint("/swagger/v2/swagger.json", "FRS.WebApi v2");
                });
            }

            app.UseHttpsRedirection();

            // 开始自定义配置
            app.UseRouting();

            #region 启用安全认证
            //添加认证中间件【必须在授权前面添加】
            app.UseAuthentication();
            //添加授权中间件
            app.UseAuthorization();
            #endregion

            //配置Cors
            app.UseCors("any");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
