using System.Collections.Generic;

namespace Markdown.TokenParser.Nodes;

public class HeadingNode : INode
{
    public List<INode>? Descendants { get; }
    public int Consumed { get; }
    public string? Value => null;

    public HeadingNode(List<INode>? descendants, int consumed)
    {
        Descendants = descendants;
        Consumed = consumed;
    }

    public TypeOfNode Type => TypeOfNode.Text;
    public bool Present => true;
    public bool Null => false;
}