using Markdown.TokenParser.Nodes;

namespace Markdown.Converter.Visitors;

public class TextVisitor : IVisitor
{
    public string Visit(INode node)
    {
        return node.Value;
    }
}