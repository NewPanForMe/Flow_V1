using BMS_Base.Config;
using BMS_Db.EfContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NLog.Extensions.Logging;
using ResetUserServer;
IHost host = Host.CreateDefaultBuilder(args).UseWindowsService()
    .ConfigureServices((hostContext , services) =>
    {
        var configuration = hostContext.Configuration;
        services.Configure<DbConfig>(configuration.GetSection("DbConfig"));
        configuration.Bind("DbConfig", DbConfig.Instance);
        Console.WriteLine("DbConfig：" + DbConfig.Instance);
        //已经注入，可以直接使用
        services.AddDbContext<BmsV1DbContext>(opt =>
        {
            opt.UseSqlServer(DbConfig.Instance.SqlServer);
        });
        services.AddLogging(log =>
        {
            log.AddNLog("nlog.config");
        });
        services.AddHostedService<Worker>();
    })
    .Build();
await host.RunAsync();
