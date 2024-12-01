using System;
using System.Collections.Generic;

namespace Markdown.TokenParser.Nodes;

public record Node(TypeOfNode Type, List<Node>? Descendants, string? Value, int Consumed)
{
    public Node(TypeOfNode type, List<Node> descendants, int consumed) 
        : this(type, descendants, null, consumed) { }
    
    public Node(TypeOfNode type, string value, int consumed) 
        : this(type, null, value, consumed) { }
}