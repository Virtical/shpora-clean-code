using Markdown.TokenParser.Nodes;

namespace Markdown.Converter;

public interface IVisitor
{
    public string Visit(Node node);
}