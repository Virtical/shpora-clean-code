using System;
using System.Collections.Generic;

namespace Markdown.TokenParser.Nodes;
public class Node
{
    public TypeOfNode Type { get; }
    public string? Value { get; }
    public int Consumed { get; }
    public List<Node>? Descendants { get; }
    
    public Node(TypeOfNode type, List<Node> descendants, string value, int consumed)
    {
        Type = type;
        Consumed = consumed;
        Value = value;
        Descendants = descendants;
    }
    
    public Node(TypeOfNode type, List<Node> descendants, int consumed)
    {
        Type = type;
        Consumed = consumed;
        Value = null;
        Descendants = descendants;
    }
    
    public Node(TypeOfNode type, string value, int consumed)
    {
        Type = type;
        Consumed = consumed;
        Value = value;
        Descendants = null;
    }
}