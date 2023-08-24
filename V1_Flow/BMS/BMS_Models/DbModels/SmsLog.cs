namespace BMS_Models.DbModels;

public record SmsLog
{
    /// <summary>
    /// 主键
    /// </summary>
    public int Id { get; set; } = 0;
    /// <summary>
    /// 编号
    /// </summary>
    public string Code { get; set; } = string.Empty;
    /// <summary>
    /// 手机号
    /// </summary>
    public string Phone { get; set; } = string.Empty;
    /// <summary>
    /// 验证码
    /// </summary>
    public string VerifyCode { get; set; } = string.Empty;
    /// <summary>
    /// 时间
    /// </summary>
    public DateTime SendTime { get; set; } = DateTime.Now;
    /// <summary>
    /// 类型
    /// </summary>
    public string Type { get; set; } = string.Empty;
    /// <summary>
    /// 接口返回结果
    /// </summary>
    public string SmsResult { get; set; } = string.Empty;
    /// <summary>
    /// 过期时间
    /// </summary>
    public DateTime ExpireDate { get; set; } = DateTime.Now;


    

}