using Ys.Tools.Config;

namespace BMS_Base.Config;

public record SystemConfig
{
    public static SystemConfig Instance { get; set; } = new SystemConfig();

    /// <summary>
    /// 账户锁定次数
    /// </summary>
    public int ErrorCount { get; set; }
    /// <summary>
    /// 文件上传文件夹
    /// </summary>
    public string UploadFileFolder { get; set; }


}