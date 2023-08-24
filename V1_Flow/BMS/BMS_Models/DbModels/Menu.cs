namespace BMS_Models.DbModels;
public class Menu
{
    public string Path { get; set; }= String.Empty;
    public string Icon { get; set; }= String.Empty;
    public string Value { get; set; }= String.Empty;
    public string Name { get; set; }= String.Empty;
    public List<Menu> Child { get; set; } = new List<Menu>();
    public Meta Meta { get; set; } = new Meta();
}
public class Meta
{
    public string Title { get; set; } = String.Empty;
}