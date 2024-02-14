using Goldlight.ServiceVirtualization.RulesSupport;
using Newtonsoft.Json.Linq;

namespace Goldlight.ServiceVirtualization.Models.Tree;

public class TreeNodeParser
{
  private readonly ItemTypeDetails itemTypeDetails = new();

  public void Parse(string json)
  {
    ArgumentException.ThrowIfNullOrWhiteSpace(json, nameof(json));
    JToken parent = JToken.Parse(json);
    AddTokenNodes(parent, "Document", Nodes);
  }

  public HashSet<TreeNode> Nodes { get; } = [];

  private void AddObjectNodes(JObject jsonObject, string name, ISet<TreeNode> parent)
  {
    var node = new TreeNode(name) { Type = itemTypeDetails.Name(ItemType.Object) };
    parent.Add(node);

    foreach (var property in jsonObject.Properties())
    {
      AddTokenNodes(property.Value, property.Name, node.Nodes);
    }
  }

  private void AddArrayNodes(JArray array, string name, ISet<TreeNode> parent)
  {
    var node = new TreeNode(name);
    parent.Add(node);

    for (var i = 0; i < array.Count; i++)
    {
      AddTokenNodes(array[i], $"[{i}]", node.Nodes);
    }
  }

  private void AddTokenNodes(JToken token, string name, ISet<TreeNode> parent)
  {
    switch (token)
    {
      case JValue value:
        parent.Add(new TreeNode(name, value));
        break;
      case JArray array:
        AddArrayNodes(array, name, parent);
        break;
      case JObject jObject:
        AddObjectNodes(jObject, name, parent);
        break;
    }
  }
}