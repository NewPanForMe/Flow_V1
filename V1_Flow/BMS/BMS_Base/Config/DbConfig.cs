namespace BMS_Base.Config;

public record DbConfig
{

    public static DbConfig Instance { get; set; } = new DbConfig();

    /// <summary>
    /// SQL Server链接字符串
    /// </summary>
    public string SqlServer { get; set; } = String.Empty;
    /// <summary>
    /// Postgre SQL链接字符串
    /// </summary>
    public string PgSql { get; set; } = String.Empty;
    /// <summary>
    /// MySql 链接字符串
    /// </summary>
    public string MySql { get; set; } = String.Empty;


    
}