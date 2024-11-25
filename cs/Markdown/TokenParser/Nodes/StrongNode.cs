using System.Collections.Generic;

namespace Markdown.TokenParser.Nodes;

public class StrongNode : INode
{
    public TypeOfNode Type { get; }
    public int Consumed { get; }
    public List<INode>? Descendants { get; }
    public string? Value => null;
    public StrongNode(TypeOfNode type, List<INode> descendants, int consumed)
    {
        Type = type;
        Descendants = descendants;
        Consumed = consumed;
    }

    public bool Null => false;
    public bool Present => true;
}