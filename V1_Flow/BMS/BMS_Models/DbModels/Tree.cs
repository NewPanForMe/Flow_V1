namespace BMS_Models.DbModels;

public class Tree
{
    public string Label { get; set; } =string.Empty;
    public string Value { get; set; } = string.Empty;
    public List<Tree> Children { get; set; }    = new List<Tree>();
}