using System.Collections.Generic;

namespace Markdown.TokenParser.Nodes;
public class BodyNode : INode
{
    public List<INode>? Descendants { get; }
    public int Consumed { get; }
    public string? Value => null;

    public BodyNode(List<INode>? descendants, int consumed)
    {
        Descendants = descendants;
        Consumed = consumed;
    }

    public bool Present => true;
    public TypeOfNode Type => TypeOfNode.Text;
    public bool Null => false;
}