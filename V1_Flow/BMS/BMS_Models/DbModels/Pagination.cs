namespace BMS_Models.DbModels;
public record Pagination
{
    public int DefaultCurrent { get; set; } = 0;
    public int DefaultPageSize { get; set; } = 0;
    public int Total { get; set; } = 0;
}

