using System;
using System.Collections.Generic;
using System.Linq;
using Markdown.TokenParser.Nodes;

namespace Markdown.Converter.Visitors;

public class StrongVisitor : IVisitor
{
    private static readonly Dictionary<TypeOfNode, Func<IVisitor>> SentenceVisitors = new()
    {
        { TypeOfNode.Emphasis, () => new EmphasisVisitor() },
        { TypeOfNode.Text, () => new TextVisitor() }
    };
    
    public string Visit(Node node)
    {
        return $"<strong>{string.Join(string.Empty, node.Descendants.Select(sentence => VisitorFor(sentence).Visit(sentence)))}</strong>";
    }
    
    private static IVisitor VisitorFor(Node node)
    {
        if (SentenceVisitors.TryGetValue(node.Type, out var visitorFactory))
        {
            return visitorFactory();
        }
        throw new ArgumentException("Invalid sentence node type");
    }
}
