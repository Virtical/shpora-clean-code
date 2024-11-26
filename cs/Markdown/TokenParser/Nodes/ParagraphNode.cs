using System.Collections.Generic;

namespace Markdown.TokenParser.Nodes;
public class ParagraphNode : INode
{
    public List<INode>? Descendants { get; }
    public int Consumed { get; }
    public string? Value => null;

    public ParagraphNode(List<INode>? descendants, int consumed)
    {
        Descendants = descendants;
        Consumed = consumed;
    }

    public TypeOfNode Type => TypeOfNode.Paragraph;
    public bool Present => true;
    public bool Null => false;
}