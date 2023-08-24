using BMS_Db.EfContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
namespace BMS_Db.WindowsServices;

/// <summary>
/// 此功能移动至WorkService，此处代码作废
/// 自动完成用户解锁的方法
/// </summary>
[Obsolete]
public class ResetUserService : BackgroundService
{

    private readonly ILogger<ResetUserService> _logger;
    private readonly BmsV1DbContext _dbContext;
    private readonly IServiceScope _scope;

    public ResetUserService(IServiceScopeFactory scopeFactory)
    {
        _scope = scopeFactory.CreateScope();
        var scopeServiceProvider = _scope.ServiceProvider;
        _dbContext = scopeServiceProvider.GetRequiredService<BmsV1DbContext>();
        _logger = scopeServiceProvider.GetRequiredService<ILogger<ResetUserService>>();
    }



    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);
            Console.WriteLine($"用户解锁服务运行中-{DateTime.Now}");
            var user = await _dbContext.User.Where(x => x.ErrorCount == 3 && x.ErrorCancelTime.Date == DateTime.Now.Date).AsNoTracking().ToListAsync(stoppingToken);
            Console.WriteLine($"需要解锁的数量{user.Count}个-{DateTime.Now}");
            if (user.Count > 0)
            {
                user.ForEach(x =>
                {
                    if (x.ErrorCancelTime.Hour == DateTime.Now.Hour
                        && x.ErrorCancelTime.Minute == DateTime.Now.Minute
                        && x.ErrorCancelTime.Second == DateTime.Now.Second)
                    {
                        _dbContext.Entry(x).State = EntityState.Detached;
                        x.ErrorCount = 0;
                        x.IsLock = false;
                        _dbContext.User.Update(x);
                        _logger.LogWarning($"用户{x.Name ?? x.LoginName}已解锁-{DateTime.Now}");
                        _dbContext.SaveChangesAsync(stoppingToken);
                        Task.Delay(500, stoppingToken);
                    }
                });

            }
        }
          // ReSharper disable once FunctionNeverReturns
  }
    public override void Dispose()
    {
        base.Dispose();
        _scope.Dispose();
    }
}