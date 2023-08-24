using System.ComponentModel.DataAnnotations.Schema;

namespace BMS_Models.DbModels;

public record User
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
    /// 登录名称
    /// </summary>
    public string LoginName { get; set; } = string.Empty;
    /// <summary>
    /// 登录密码
    /// </summary>
    public string LoginPassword { get; set; } = string.Empty;
    /// <summary>
    /// 密码盐
    /// </summary>
    public string LoginPasswordSalt { get; set; } = string.Empty;
    /// <summary>
    /// 姓名
    /// </summary>
    public string? Name { get; set; }
    /// <summary>
    /// 性别
    /// </summary>
    public string? Gender { get; set; } 
    /// <summary>
    /// 身份证好
    /// </summary>
    public string? IdCard { get; set; }
    /// <summary>
    /// 电话
    /// </summary>
    public string? Phone { get; set; } 
    /// <summary>
    /// 邮件
    /// </summary>
    public string? Mail { get; set; }
    /// <summary>
    /// token版本
    /// </summary>
    public int JwtVersion { get; set; } = 0;
    /// <summary>
    /// 是否删除 true-删除
    /// </summary>
    public bool IsDelete { get; set; } = false;
    /// <summary>
    /// 是否锁定 
    /// </summary>
    public bool IsLock { get; set; } = false;
    /// <summary>
    /// 密码错误次数
    /// </summary>
    public int ErrorCount { get; set; } = 0;
    /// <summary>
    /// 解锁时间
    /// </summary>
    public DateTime ErrorCancelTime { get; set; } = new DateTime();

    /// <summary>
    /// 角色
    /// </summary>
    [NotMapped]
    public string[] Roles { get; set; } = {};

    public List<string> RoleList()
    {
        return Roles.ToList();
    }


}
