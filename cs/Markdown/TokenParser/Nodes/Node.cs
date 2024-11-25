using System;
using System.Collections.Generic;

namespace Markdown.TokenParser.Nodes;
public class Node : INode
{
    public TypeOfNode Type { get; }
    public string? Value { get; }
    public int Consumed { get; }

    public List<INode>? Descendants => null;

    public Node(TypeOfNode type, string? value, int consumed)
    {
        Type = type;
        Value = value ?? throw new ArgumentNullException(nameof(value));
        Consumed = consumed;
    }

    public bool Null => false;
    public bool Present => true;
}