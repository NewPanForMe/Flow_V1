namespace BMS_Models.DbModels;

public record FileUpload
{

    //Id int Unchecked
    //Code varchar(50) Unchecked
    //    UserCode    varchar(50) Checked
    //    UserName    varchar(50) Checked
    //    FullName    varchar(50) Checked
    //    Location    varchar(300)    Checked
    //    CreateDate  datetime Checked

    /// <summary>
    /// 主键
    /// </summary>
    public int Id { get; set; } = 0;
    /// <summary>
    /// 编号
    /// </summary>
    public string Code { get; set; } = string.Empty;
    /// <summary>
    /// 上传人
    /// </summary>
    public string UserCode { get; set; } = string.Empty;
    /// <summary>
    /// 上传人
    /// </summary>
    public string UserName { get; set; } = string.Empty;
    /// <summary>
    /// 文件名称
    /// </summary>
    public string FullName { get; set; } = string.Empty;
    /// <summary>
    /// 文件存放位置
    /// </summary>
    public string Location { get; set; } = string.Empty;
    /// <summary>
    /// 上传时间
    /// </summary>
    public DateTime CreateDate { get; set; } = DateTime.Now;

}