using System.Collections.Generic;

namespace Markdown.TokenParser.Nodes;

public interface INode
{
    public TypeOfNode Type { get;  }
    public bool Null { get; }
    public bool Present { get; }
    public int Consumed { get; }
    public string? Value { get; }
    public List<INode>? Descendants { get; }
}