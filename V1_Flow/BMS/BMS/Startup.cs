using System.Reflection;
using System.Text;
using BMS_Base.Config;
using BMS_Db.EfContext;
using Consul;
using Microsoft.EntityFrameworkCore;
using NLog.Extensions.Logging;
using Ys.Tools.MiddleWare;
using Ys.Tools.Config;
using Ys.Tools.Interface;
using BMS_SMS.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.IdentityModel.Tokens;

namespace BMS;

public class Startup
{

    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        this._configuration = configuration;
    }

    // This method gets called by the runtime. Use this method to add services to the container.
    // 该方法由运行时调用，使用该方法向DI容器添加服务
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddMvc();
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAllOrigin", builder =>
            {
                builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod(); ;
            });
        });
        // 注入Jwt
        services.AddAuthentication(option =>
        {
            //认证middleware配置
            option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(config =>
        {
            config.TokenValidationParameters = new TokenValidationParameters
            {
                //Token颁发机构
                ValidIssuer = TokenConfig.Instance.IsUser,
                //颁发给谁
                ValidAudience = TokenConfig.Instance.Audience,
                //这里的key要进行加密
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(TokenConfig.Instance.Key)),
                //是否验证Token有效期，使用当前时间与Token的Claims中的NotBefore和Expires对比
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
            };
            config.SaveToken = true;
        });

        services.AddMemoryCache();

        services.Configure<FormOptions>(x =>
        {
            x.MultipartHeadersLengthLimit = 300000000;//文件最大300M
        });


        //services.AddHostedService<ResetUserService>();
        RegisterNLog(services);
        //注入数据库
        RegisterDb(services);
        //自动实现类注入
        RegisterIBll(services);
        //获取consulConfig
        RegisterConsul(services);
        //获取TokenConfig
        RegisterToken(services);
        //获取SystemConfig
        RegisterSystem(services);
        //获取SmsBaseConfig
        RegisterSms(services);



    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    // 该方法由运行时调用，使用该方法配置HTTP请求管道
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime appLifetime)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }


        app.UseRouting();
        //UseCors 必须放在 之后 UseRouting 和之前 UseAuthorization。
        //这是为了确保 CORS 标头包含在已授权和未经授权的调用的响应中。
        app.UseCors("AllowAllOrigin");
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseResponseCaching();
        app.UseMiddleware<ExceptionMiddleWare>();


        // app.UseMiddleware<JwtVersionMiddleWare>();

        app.UseEndpoints(x =>
        {
            x.MapControllers();
            x.MapGet("/", async context =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
        });
        //配置Consul
        RegisterConsul(appLifetime);
    }
    /// <summary>
    /// 注册consul
    /// </summary>
    /// <param name="appLifetime"></param>
    private static void RegisterConsul(IHostApplicationLifetime appLifetime)
    {
        using var client = new ConsulClient(x => x.Address = new Uri(ConsulConfig.Instance.BaseUrl));
        var check = new AgentServiceCheck()
        {
            DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),//服务停止后，5s开始接触注册
            HTTP = ConsulConfig.Instance.CheckApi,//健康检查
            Interval = TimeSpan.FromSeconds(10),//每10s轮询一次健康检查
            Timeout = TimeSpan.FromSeconds(5),
        };
        var service = new AgentServiceRegistration()
        {
            Checks = new[] { check },
            ID = Guid.NewGuid().ToString(),
            Name = ConsulConfig.Instance.ServiceName,
            Port = ConsulConfig.Instance.Port,
            Address = ConsulConfig.Instance.Address
        };
        client.Agent.ServiceRegister(service).Wait();
        appLifetime.ApplicationStopped.Register(() =>
        {
            Console.WriteLine("服务停止中");
            using var consulClient = new ConsulClient(x => x.Address = new Uri(ConsulConfig.Instance.BaseUrl));
            consulClient.Agent.ServiceDeregister(service.ID).Wait();
        });
    }

    /// <summary>
    /// 自动依赖注入
    /// </summary>
    /// <param name="service"></param>
    private static void RegisterIBll(IServiceCollection service)
    {
        var assemblies = Assembly.GetEntryAssembly()?.GetReferencedAssemblies().Select(Assembly.Load).ToList();
        assemblies?.ForEach(assembly =>
        {
            var list2 = assembly.GetTypes().Where(x => x.IsClass && x.GetInterfaces().Any(y => y == typeof(IBll))).ToList();
            list2.ForEach(type =>
            {
                service.AddScoped(type);
            });
        });
        assemblies?.ForEach(assembly =>
        {
            var list2 = assembly.GetTypes().Where(x => x.IsClass && x.GetInterfaces().Any(y => y == typeof(IStaticBll))).ToList();
            list2.ForEach(type =>
            {
                service.AddSingleton(type);
            });
        });


    }

    /// <summary>
    /// 注入数据库
    /// </summary>
    /// <param name="service"></param>
    private void RegisterDb(IServiceCollection service)
    {
        service.Configure<DbConfig>(_configuration.GetSection("DbConfig"));
        _configuration.Bind("DbConfig", DbConfig.Instance);
        //已经注入，可以直接使用
        service.AddDbContext<BmsV1DbContext>(opt =>
        {
            opt.UseSqlServer(DbConfig.Instance.SqlServer);
        });
    }

    /// <summary>
    /// 获取consulConfig
    /// </summary>
    /// <param name="service"></param>
    private void RegisterConsul(IServiceCollection service)
    {
        //获取配置文件信息
        service.Configure<ConsulConfig>(_configuration.GetSection("ConsulConfig"));
        _configuration.Bind("ConsulConfig", ConsulConfig.Instance);
    }

    /// <summary>
    /// 配置Nlog
    /// </summary>
    /// <param name="service"></param>
    private static void RegisterNLog(IServiceCollection service)
    {
        service.AddLogging(log =>
        {
            log.AddNLog("nlog.config");
        });
    }
    /// <summary>
    /// 注入Token
    /// </summary>
    /// <param name="service"></param>
    private void RegisterToken(IServiceCollection service)
    {
        service.Configure<TokenConfig>(_configuration.GetSection("TokenConfig"));
        _configuration.Bind("TokenConfig", TokenConfig.Instance);

    }

    /// <summary>
    /// 注入SyStem
    /// </summary>
    /// <param name="service"></param>
    private void RegisterSystem(IServiceCollection service)
    {
        service.Configure<SystemConfig>(_configuration.GetSection("SystemConfig"));
        _configuration.Bind("SystemConfig", SystemConfig.Instance);

    }
    /// <summary>
    /// 注入SMS
    /// </summary>
    /// <param name="service"></param>
    private void RegisterSms(IServiceCollection service)
    {
        service.Configure<SmsBaseConfig>(_configuration.GetSection("SmsBaseConfig"));
        _configuration.Bind("SmsBaseConfig", SmsBaseConfig.Instance);
    }
}
