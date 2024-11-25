using System.Linq;
using Markdown.TokenParser.Nodes;

namespace Markdown.Converter.Visitors;

public class BodyVisitor : IVisitor
{
    private readonly ParagraphVisitor paragraphVisitor = new();
    
    public string Visit(INode bodyNode)
    {
        return string.Join(string.Empty, bodyNode.Descendants.Select(paragraph => paragraphVisitor.Visit(paragraph)));
    }
}