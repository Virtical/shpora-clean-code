using System.Collections.Generic;

namespace Markdown.TokenParser.Nodes;

public class NullNode : INode
{
    public bool Null => true;
    public bool Present => false;
    public string? Value => null;
    public int Consumed => 0;
    public TypeOfNode Type => TypeOfNode.Text;
    public List<INode>? Descendants => null;
}