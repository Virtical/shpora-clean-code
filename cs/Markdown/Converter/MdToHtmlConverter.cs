using Markdown.Converter.Visitors;
using Markdown.TokenParser.Nodes;

namespace Markdown.Converter;
public class MdToHtmlConverter : IMdConverter
{
    public string RenderTokens(INode tokens)
    {
        var bodyVisitors = new BodyVisitor();
        
        return bodyVisitors.Visit(tokens);
    }
}
