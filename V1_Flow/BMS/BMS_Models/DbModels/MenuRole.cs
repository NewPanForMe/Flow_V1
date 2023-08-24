namespace BMS_Models.DbModels;

public class MenuRole
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
    /// 模块编号
    /// </summary>
    public string ModuleCode { get; set; } = string.Empty;    
    /// <summary>
    /// 角色编号
    /// </summary>
    public string RoleCode { get; set; } = string.Empty;
}