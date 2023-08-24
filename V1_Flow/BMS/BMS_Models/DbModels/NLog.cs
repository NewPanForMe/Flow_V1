namespace BMS_Models.DbModels;

public class NLog
{
    public long Id { get; set; } =0;
    public string Application { get; set; } = string.Empty;
    public string Level { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string Logger { get; set; } = string.Empty;
    public string CallSite { get; set; } = string.Empty;
    public string Exception { get; set; } = string.Empty;
    public DateTime Logged { get; set; } = new DateTime();
}