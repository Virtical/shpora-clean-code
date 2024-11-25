using Markdown.TokenParser.Nodes;

namespace Markdown.Converter;

public interface IMdConverter
{
    string RenderTokens(INode tokens);
}