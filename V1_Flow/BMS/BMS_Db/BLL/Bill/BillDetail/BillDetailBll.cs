using BMS_Db.BLL.Sys.Sms;
using BMS_Db.EfContext;
using BMS_Models.DbModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using Ys.Tools.Extra;
using Ys.Tools.Interface;
using Ys.Tools.Models;
using Ys.Tools.Response;

namespace BMS_Db.BLL.Bill.BillDetail;

public class BillDetailBll : IBll
{
    private readonly BmsV1DbContext _dbContext;
    private readonly ILogger<BillDetailBll> _logger;
    private readonly string[] _detailOutStatus = new string[]
    {
        "交易成功", "还款成功", "充值完成", "支付成功","已转账","对方已收钱"
    };
    public BillDetailBll(BmsV1DbContext dbContext, ILogger<BillDetailBll> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
        _dbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }

    /// <summary>
    /// 获取列表
    /// </summary>
    /// <returns></returns>
    public List<BMS_Models.DbModels.BillDetail> GetBillDetails(string billCode)
    {
        var listAsync = _dbContext.BillDetail.AsNoTracking()
            .OrderByDescending(x => x.Money).ThenByDescending(x=>x.Date).ToList();

        if (!string.IsNullOrEmpty(billCode))
        {
            listAsync = listAsync.Where(x => x.BillCode == billCode).ToList();
        }
        return listAsync;
    }
    /// <summary>
    /// 获取单条
    /// </summary>
    /// <returns></returns>
    public BMS_Models.DbModels.BillDetail GetBillDetailEntityByCode(string code)
    {
        code.NotNull("传入编号为空");
        var bill = _dbContext.BillDetail.FirstOrDefault(x => x.Code.Equals(code)).NotNull("当前数据不存在");
        return bill;
    }

    /// <summary>
    /// 获取消费类型图标
    /// </summary>
    /// <returns></returns>
    public async Task<ApiResult> GetTypeChart(CurrentUser current, int year, int month,string type)
    {
        var list = from detail in _dbContext.BillDetail
                   join billx in _dbContext.Bill on detail.BillCode equals billx.Code
                   where billx.UserCode.Equals(current.Code) 
                         && billx.Year == year 
                         && billx.Month == month
                         && billx.Type == type
                         && _detailOutStatus.Contains(detail.Status)
                         && detail.InOut == "支出"
                   select new
                   {
                       detail.Type,
                       detail.Money
                   };
        var bill = await list.GroupBy(x => x.Type)
                        .Select(x =>
                            new
                            {
                                Value = x.Sum(y => y.Money),
                                Name = x.Key,
                            }).ToListAsync(); ;
        return ApiResult.True(new { data = bill });
    }

    /// <summary>
    /// 获取支付类型总和图标
    /// </summary>
    public async Task<ApiResult> GetPayTypeChart(CurrentUser current, int year, int month)
    {
        var list = from billx in _dbContext.Bill
                   join detail in _dbContext.BillDetail on billx.Code equals detail.BillCode
                   where billx.UserCode.Equals(current.Code)
                         && billx.Year== year  
                         && billx.Month== month
                         && _detailOutStatus.Contains(detail.Status)
                         && detail.InOut== "支出"
                   select new
                   {
                       billx.Type,
                       detail.Money,
                   };
        var bill = await list.GroupBy(x => new
        {
            x.Type,
        })
            .Select(x =>
                new
                {
                    Money = x.Sum(y => y.Money),
                    Type = x.Key.Type,
                }).ToListAsync(); ;
        return ApiResult.True(new { data = bill });
    }

    /// <summary>
    /// 获取收入信息
    /// </summary>
    /// <param name="current"></param>
    /// <param name="year"></param>
    /// <param name="month"></param>
    /// <returns></returns>

    public async Task<ApiResult> GetInChart(CurrentUser current, int year, int month,string type)
    {
        var list = from detail in _dbContext.BillDetail
            join billx in _dbContext.Bill on detail.BillCode equals billx.Code
            where billx.UserCode.Equals(current.Code)
                  && billx.Year == year
                  && billx.Month == month
                  && billx.Type == type
                  && detail.InOut == "收入"
            select new
            {
                detail.Type,
                detail.Money
            };
        var bill = await list.GroupBy(x => x.Type)
            .Select(x =>
                new
                {
                    Value = x.Sum(y => y.Money),
                    Name = x.Key,
                }).ToListAsync(); ;
        return ApiResult.True(new { data = bill });
    }


}