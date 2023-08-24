using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace BMS_Models.DbModels;

public class Role
{
    /// <summary>
    /// 主键
    /// </summary>
    public int Id { get; set; } = 0;
    /// <summary>
    /// 编号
    /// </summary>
    public string Code { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// 角色名称
    /// </summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// 排序
    /// </summary>
    public int OrderBy { get; set; } = 0;

    /// <summary>
    /// 模块
    /// </summary>
    [NotMapped]
    public string[] Modules { get; set; } = new string[]{};

    public List<string> ModuleList()
    {
        return Modules.ToList();
    }

}