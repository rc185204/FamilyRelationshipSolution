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
        /// log4net �ִ���
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

            #region Swagger����

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "FRS.WebApi", Version = "v1" });
                c.SwaggerDoc("v2", new OpenApiInfo { Title = "FRS.WebApi", Version = "v2" });// API ����
                // ��һ��controller�г��ֶ��[httpPost]/[httpGet]ʱ��Ĭ��ʹ�õ�һ��
                // ��ȷ�������һ��controller��ֻ�ܰ���һ��get/post/put/delete,���ܶ��get���ֵ�ʱ���ǿ��Եģ�����Swagger����֧������������
                // ���ǽ��齫���action ��ֳɶ��controller
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

                // ��ʾ��������<summary>˵��
                // 1.��������������xml
                // 2.c.IncludeXmlComments
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

                //Swagger�����Զ�������UI
                //c.OperationFilter<AuthTokenHeaderParameter>();
                c.OperationFilter<ApiVersionHeaderParameter>();

                //��UI������Ȩ��֤��ť
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "��Ȩ��֤, ����Bearer {token}(ע������֮����һ���ո�) \"",
                    Name = "Authorization",//jwtĬ�ϵĲ�������
                    In = ParameterLocation.Header,//jwtĬ�ϴ��Authorization��Ϣ��λ��(����ͷ��)
                    Scheme = "Bearer",
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT"
                });

                //�����з�������Ϊ����bearerͷ����Ϣ
                //ÿ��API�����󣬻���һ�����ı�־�������÷����ᴫ��bearer token��
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
                // ��Swagger����FileUploadOperation
                c.OperationFilter<FileUploadOperation>();
            });

            #endregion

            #region ��� jwt ��֤
            services.Configure<JwtConfig.JwtConfig>(Configuration.GetSection("JwtConfig"));
            var jwtConfig = new JwtConfig.JwtConfig();
            Configuration.Bind("JwtConfig", jwtConfig);
            GenerateJwt.SetJwtConfig(jwtConfig);

            services.AddAuthentication(option =>
            {
                //��֤middleware����
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                //Ĭ�Ϸ���
                option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            })
               .AddJwtBearer(options =>
               {
                   options.TokenValidationParameters = new TokenValidationParameters
                   {
                       //ע�����ǻ������ʱ�䣬�ܵ���Чʱ��������ʱ�����jwt�Ĺ���ʱ�䣬��������ã�Ĭ����5����
                       ClockSkew = TimeSpan.FromSeconds(4),

                       //Token�䷢����
                       ValidIssuer = jwtConfig.Issuer,
                       //�䷢��˭
                       ValidAudience = jwtConfig.Audience,
                       ValidateIssuer = true,
                       ValidateAudience = true,
                       ValidateLifetime = true, //�Ƿ���֤Token��Ч�ڣ�ʹ�õ�ǰʱ����Token��Claims�е�NotBefore��Expires�Ա�
                                                //�����keyҪ���м���
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.SecretKey)),
                   };
                   options.Events = new JwtBearerEvents
                   {
                       OnAuthenticationFailed = context =>
                       {
                           // ������ڣ������ӵ�������ͷ��Ϣ��
                           if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                           {
                               context.Response.Headers.Add("Token-Expired", "true");

                               // �������Access-Control-Allow-* �����п�����֤�������м��룬����tokenʧЧ�󣬻᷵�ؿ������
                               context.Response.Headers.Add("Access-Control-Allow-Credentials", "true");
                               string Origin = context.Request.Headers["origin"];
                               context.Response.Headers.Add("Access-Control-Allow-Origin", Origin);

                               // ������tokenʧЧ��ķ��������У�������������ȡ��headers�ı�ǩ
                               context.Response.Headers.Add("Access-Control-Allow-Headers", "Token-Expired");
                               context.Response.Headers.Add("Access-Control-Expose-Headers", "Token-Expired,Content-Type");
                           }
                           return Task.CompletedTask;
                       }
                   };
               });
            #endregion

            #region json�ַ�����
            //���� .NET Core WebApi�з��� json ��������ĸ��Сд�����ֶ�ԭʼ��Сд�� �����õ������Ĭ������ĸСд
            services.AddControllers().AddJsonOptions(config =>
            {
                config.JsonSerializerOptions.PropertyNamingPolicy = null;
            });

            //��swagger�н�ö����ʾΪ�ַ���
            services.AddControllers().AddJsonOptions(options =>
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
            #endregion

            # region ��־ע��
            // AddSingleton ��Ŀ����-��Ŀ�ر�.�൱�ھ�̬��,ֻ����һ��
            services.AddSingleton<ILoggerHelper, LoggerHelper>();

            //log4net
            repository = LogManager.CreateRepository("FRS.WebApi");//��Ŀ����
            XmlConfigurator.Configure(repository, new FileInfo("log4net.config"));//ָ�������ļ�
            #endregion

            #region ���쳣������ע�뵽������
            // AddScoped : ����ʼ-�������  ����������л�ȡ�Ķ�����ͬһ��������������ʱ�����������
            services.AddScoped<GlobalExceptionFilter>();
            #endregion

            #region ע��ѹ��������
            services.AddScoped<CompressionAttribute>();
            #endregion

            #region ���cors���� ���ÿ�����
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
                    .WithOrigins(originsarray)//�����κ���Դ����������
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();//ָ������cookie
                });
            });
            #endregion

            #region �汾����
            services.AddApiVersioning(opt =>
            {
                opt.AssumeDefaultVersionWhenUnspecified = true;
                opt.DefaultApiVersion = new ApiVersion(1, 0);
                opt.ApiVersionReader = ApiVersionReader.Combine(
                    //new MediaTypeApiVersionReader("version")// ͨ��MediaType,��������2��ͬ�� Controller ��ͨ��content-type�������İ汾�ŷ��ض�Ӧ��controller,����������汾�ţ��򷵻����°汾�Ŷ�Ӧ��controller
                    //new QueryStringApiVersionReader(parameterNames:"version"),// ͨ����ѯ�ַ������ư汾��https://localhost:44392/api/Version?version=1.0
                    new HeaderApiVersionReader("api-version")//ͨ��header�����汾��
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

                // ���𻷾��¿���Swagger��Ϊ���ڲ����ŶӺ���ʱ�������Ŷӳ�Ա���ڲ����������ʵ���
                // ʵ�ʲ�������������Ҫ�õ�
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "FRS.WebApi v1");
                    c.SwaggerEndpoint("/swagger/v2/swagger.json", "FRS.WebApi v2");
                });
            }

            app.UseHttpsRedirection();

            // ��ʼ�Զ�������
            app.UseRouting();

            #region ���ð�ȫ��֤
            //�����֤�м������������Ȩǰ����ӡ�
            app.UseAuthentication();
            //�����Ȩ�м��
            app.UseAuthorization();
            #endregion

            //����Cors
            app.UseCors("any");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
