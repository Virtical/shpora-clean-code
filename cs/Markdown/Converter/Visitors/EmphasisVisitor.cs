using System.Linq;
using Markdown.TokenParser.Nodes;

namespace Markdown.Converter.Visitors;

public class EmphasisVisitor : IVisitor
{
    private readonly TextVisitor textVisitor = new();
    
    public string Visit(INode node)
    {
        return $"<em>{string.Join(string.Empty, node.Descendants.Select(sentence => textVisitor.Visit(sentence)))}</em>";
    }
}