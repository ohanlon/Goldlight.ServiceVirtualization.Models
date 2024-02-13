using Newtonsoft.Json.Linq;

namespace Goldlight.ServiceVirtualization.Models.Tree;

public class TreeNode
{
  public TreeNode()
  {
  }

  public TreeNode(string name)
  {
    Name = name;
  }

  public TreeNode(string name, JValue value)
  {
    Name = name;
    Type = value.Type.ToString();
    if (value.Value is null) return;
    switch (value.Type)
    {
      case JTokenType.Integer:
        LongValue = (long)value.Value;
        break;
      case JTokenType.Float:
        FloatingPointValue = (double)value.Value;
        break;
      case JTokenType.Date:
        DateTimeValue = DateTime.Parse(value.Value.ToString()!);
        break;
      case JTokenType.Boolean:
        BoolValue = (bool)value.Value;
        break;
      case JTokenType.Comment:
      case JTokenType.Constructor:
      case JTokenType.Undefined:
        break;
      case JTokenType.Null:
        Value = string.Empty;
        break;
      default:
        Value = value.Value.ToString();
        break;
    }
  }

  public bool IsExpanded { get; set; } = true;
  public string Rule { get; set; } = "Equals";
  public string? Name { get; set; }
  public string? Type { get; set; }
  public DateTime? DateTimeValue { get; set; }
  public string? Value { get; set; }
  public long? LongValue { get; set; }
  public double? FloatingPointValue { get; set; }
  public bool? BoolValue { get; set; }
  public HashSet<TreeNode> Nodes { get; set; } = new();
}