namespace BMS_Base.Config;
public record ConsulConfig
{
    public static ConsulConfig Instance { get; set; } = new ConsulConfig();
    /// <summary>
    /// Consul服务名称
    /// </summary>
    public string ServiceName { get; set; } = string.Empty;
    /// <summary>
    /// 健康检查API
    /// </summary>
    public string CheckApi { get; set; } = string.Empty;
    /// <summary>
    /// 地址-项目启动地址
    /// 也是Consul服务挂的地址
    /// </summary>
    public string Address { get; set; } = string.Empty;
    /// <summary>
    /// 运行端口
    /// </summary>
    public int Port { get; set; } = 0;
    /// <summary>
    /// 基础url
    /// </summary>
    public string BaseUrl { get; set; } = string.Empty;


}
