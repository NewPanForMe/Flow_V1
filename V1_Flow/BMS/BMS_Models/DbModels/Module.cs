namespace BMS_Models.DbModels;

public record Module
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
    /// 菜单路径
    /// </summary>
    public string Path { get; set; } = string.Empty;
    /// <summary>
    /// 菜单图标
    /// </summary>
    public string Icon { get; set; } = string.Empty;
    /// <summary>
    /// 菜单值
    /// </summary>
    public string Value { get; set; } = string.Empty;
    /// <summary>
    /// 菜单名称
    /// </summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// 父节点名称
    /// </summary>
    public string ParentCode { get; set; } = string.Empty;
    /// <summary>
    /// 是否展示
    /// </summary>
    public bool IsShow { get; set; } = true;
    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime? CreateTime { get; set; } 
    //public MetaClass Meta { get; set; } =new MetaClass();
}

public class MetaClass
{
    /// <summary>
    /// 菜单名称
    /// </summary>
    public string Title { get; set; } = string.Empty;
}

public class ModuleGroup
{
    public string Group { get; set; } = String.Empty;
    public string Code { get; set; } = String.Empty;
    public List<SelectOption> Children { get; set; } = new List<SelectOption>();
}

