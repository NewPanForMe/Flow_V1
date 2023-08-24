using BMS_Db.BLL.Sys.Sms;
using BMS_Db.EfContext;
using BMS_Models.DbModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ys.Tools.Extra;
using Ys.Tools.Interface;
using Ys.Tools.Models;
using Ys.Tools.MoreTool;
using Ys.Tools.Response;

namespace BMS_Db.BLL.Bill.Bill;

/// <summary>
/// 账单类
/// </summary>
public class BillBll : IBll
{
    private readonly BmsV1DbContext _dbContext;
    private readonly ILogger<BillBll> _logger;
    private readonly string[] _detailOutStatus = new string[]
    {
        "交易成功", "还款成功", "充值完成", "支付成功","已转账","对方已收钱"
    };
    public BillBll(BmsV1DbContext dbContext, ILogger<BillBll> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
        _dbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }


    /// <summary>
    /// 数据处理
    /// </summary>
    /// <param name="year"></param>
    /// <param name="month"></param>
    /// <param name="path"></param>
    /// <param name="current"></param>
    /// <param name="type">判断是微信还是支付宝</param>
    public async Task<ApiResult> BillAdd(int year, int month, string path, string type, CurrentUser current)
    {
        var isExist = _dbContext.Bill.Any(x=>x.UserCode==current.Code && x.Year==year && x.Month==month && x.Type==type);
        if (isExist)
        {
            return ApiResult.False($"{current.Name}的{year}年{month}月{type}账单已上传");
        }
        var bill = new BMS_Models.DbModels.Bill()
        {
            Code = Guid.NewGuid().ToString(),
            UserName = current.Name,
            UserCode = current.Code,
            CreateDate = DateTime.Now,
            Month = month,
            Year = year,
            Type = type
        };
        var start = 0;
        if (type == "微信") start = 17;
        if (type == "支付宝") start = 25;
        var readCsv = ExcelTools.ReadCsv(path);
        var desc = "";
        for (var i = 0; i < start - 1; i++)
        {
            //读取备注信息
            desc += readCsv[i].A + "\r\n";
        }
        bill.Remark = desc;
      
        var detailList = new List<BMS_Models.DbModels.BillDetail>();
        for (var i = start; i < readCsv.Count; i++)
        {
            //表格信息
            BMS_Models.DbModels.BillDetail billDetail;
            if (type == "微信")
            {
                string money = readCsv[i].F;
                money = money.Replace("¥", "");
                billDetail = new BMS_Models.DbModels.BillDetail()
                {//"9.90"
                    Code = Guid.NewGuid().ToString(),
                    BillCode = bill.Code,
                    Date = DateTime.Parse(readCsv[i].A),
                    Type = readCsv[i].B,
                    BTo = readCsv[i].C,
                    Describe = readCsv[i].D,
                    InOut = readCsv[i].E,
                    Money = decimal.Parse(money),
                    PayMethod = readCsv[i].G,
                    Status = readCsv[i].H,
                    TranOrderNo = readCsv[i].I,
                    MerOrderNo = readCsv[i].K,
                    Remark = readCsv[i].M,
                    IsDev = false
                };
            }
            else
            {
                billDetail = new BMS_Models.DbModels.BillDetail()
                {
                    Code = Guid.NewGuid().ToString(),
                    BillCode = bill.Code,
                    Date = DateTime.Parse(readCsv[i].A),
                    Type = readCsv[i].B,
                    BTo = readCsv[i].C,

                    ToAccount = readCsv[i].D,
                    Describe = readCsv[i].E,
                    InOut = readCsv[i].F,
                    Money = decimal.Parse(readCsv[i].G),
                    PayMethod = readCsv[i].H,
                    Status = readCsv[i].I,
                    TranOrderNo = readCsv[i].J,
                    MerOrderNo = readCsv[i].L,
                    Remark = readCsv[i].N,
                    IsDev = false
                };

            }
            detailList.Add(billDetail);
        }

        bill.InMoney= detailList.Where(x => x.InOut == "收入").Select(x => x.Money).Sum();
        bill.OutMoney= detailList.Where(x => _detailOutStatus.Contains(x.Status)).Select(x => x.Money).Sum();
        await _dbContext.Bill.AddAsync(bill);
        await _dbContext.BillDetail.AddRangeAsync(detailList);
        return ApiResult.True(new { code = bill.Code });
    }


    /// <summary>
    /// 获取列表
    /// </summary>
    /// <param name="userCode">用户编号</param>
    /// <returns></returns>
    public async Task<List<BMS_Models.DbModels.Bill>> GetBills(string userCode)
    {
        var listAsync = await _dbContext.Bill.Where(x=>x.UserCode==userCode).AsNoTracking().ToListAsync();
        return listAsync;
    }


    /// <summary>
    /// 获取单条
    /// </summary>
    /// <returns></returns>
    public BMS_Models.DbModels.Bill GetBillEntityByCode(string code)
    {
        code.NotNull("传入编号为空");
        var bill = _dbContext.Bill.FirstOrDefault(x => x.Code.Equals(code)).NotNull("当前数据不存在");
        return bill;
    }



}