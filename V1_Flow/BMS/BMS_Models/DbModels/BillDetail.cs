namespace BMS_Models.DbModels;

public class BillDetail
{
    /// <summary>
    /// 编号
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// 主键
    /// </summary>
    public string Code { get; set; } = string.Empty;
    /// <summary>
    /// 绑定主表
    /// </summary>
    public string BillCode { get; set; } = string.Empty;
    /// <summary>
    /// 交易时间
    /// </summary>
    public DateTime Date { get; set; }
    /// <summary>
    /// 交易分类
    /// </summary>
    public string Type { get; set; } = string.Empty;
    /// <summary>
    /// 交易对方
    /// </summary>
    public string BTo { get; set; } = string.Empty;
    /// <summary>
    /// 对方账号
    /// </summary>
    public string ToAccount { get; set; } = string.Empty;
    /// <summary>
    /// 商品说明
    /// </summary>
    public string Describe { get; set; } = string.Empty;
    /// <summary>
    /// 收/支
    /// </summary>
    public string InOut { get; set; } = string.Empty;
    /// <summary>
    /// 金额
    /// </summary>
    public decimal Money { get; set; }
    /// <summary>
    /// 收/付款方式
    /// </summary>
    public string PayMethod { get; set; } = string.Empty;
    /// <summary>
    /// 交易状态
    /// </summary>
    public string Status { get; set; } = string.Empty;
    /// <summary>
    /// 交易订单号
    /// </summary>
    public string TranOrderNo { get; set; } = string.Empty;
    /// <summary>
    /// 商家订单号
    /// </summary>
    public string MerOrderNo { get; set; } = string.Empty;
    /// <summary>
    /// 备注
    /// </summary>
    public string Remark { get; set; } = string.Empty;
    /// <summary>
    /// 是否正式数据
    /// </summary>
    public bool IsDev { get; set; } 
}