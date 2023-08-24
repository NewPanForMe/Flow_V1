using BMS_Db.EfContext;
using BMS_Models.DbModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ys.Tools.Interface;
namespace BMS_Db.BLL.Sys.NLog;

public class LogBll : IBll
{
    private readonly BmsV1DbContext _dbContext;
    private readonly ILogger<LogBll> _logger;
    public LogBll(BmsV1DbContext dbContext, ILogger<LogBll> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
        _dbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }

    /// <summary>
    /// 获取日志
    /// </summary>
    /// <param name="count">列表获取数量</param>
    /// <returns></returns>
    public async Task<List<BMS_Models.DbModels.NLog>> GetLogs(int count)
    {
        var listAsync = await _dbContext.NLog.OrderByDescending(x => x.Logged).Take(count).ToListAsync();
        return listAsync;
    }


}