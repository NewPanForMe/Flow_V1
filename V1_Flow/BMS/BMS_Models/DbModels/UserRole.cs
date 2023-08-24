namespace BMS_Models.DbModels;

public class UserRole
{
    /// <summary>
    /// 编号
    /// </summary>
    public int Id { get; set; } = 0;
    /// <summary>
    /// 主键
    /// </summary>
    public string Code { get; set; } = string.Empty;
    /// <summary>
    /// 用户编号
    /// </summary>
    public string UserCode { get; set; } = string.Empty;
    /// <summary>
    /// 角色编号
    /// </summary>
    public string RoleCode { get; set; } = string.Empty;
    /// <summary>
    /// 角色名称
    /// </summary>
    public string RoleName { get; set; } = string.Empty;
}