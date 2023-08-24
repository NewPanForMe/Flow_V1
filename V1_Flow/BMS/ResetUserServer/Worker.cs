using BMS_Db.EfContext;
using BMS_Db.WindowsServices;
using BMS_Models.DbModels;
using Microsoft.EntityFrameworkCore;

namespace ResetUserServer
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly BmsV1DbContext _dbContext;
        private readonly IServiceScope _scope;

        public Worker(IServiceScopeFactory scopeFactory)
        {
            _scope = scopeFactory.CreateScope();
            var scopeServiceProvider = _scope.ServiceProvider;
            _dbContext = scopeServiceProvider.GetRequiredService<BmsV1DbContext>();
            _logger = scopeServiceProvider.GetRequiredService<ILogger<Worker>>();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
                Console.WriteLine($"用户解锁服务运行中-{DateTime.Now}");
                var user = await _dbContext.User.Where(x => x.ErrorCount == 3 && x.ErrorCancelTime.Date == DateTime.Now.Date).ToListAsync(stoppingToken);
                if (user.Count > 0)
                {
                    async void Action(User x)
                    {
                        if (x.ErrorCancelTime.Hour == DateTime.Now.Hour && x.ErrorCancelTime.Minute == DateTime.Now.Minute && x.ErrorCancelTime.Second == DateTime.Now.Second)
                        {
                            x.ErrorCount = 0;
                            x.IsLock = false;
                            _dbContext.User.Update(x);
                            _logger.LogWarning($"用户{x.Name ?? x.LoginName}已解锁-{DateTime.Now}");
                            await _dbContext.SaveChangesAsync(stoppingToken);
                            await Task.Delay(500, stoppingToken);
                        }
                    }
                    user.ForEach(Action);
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
}