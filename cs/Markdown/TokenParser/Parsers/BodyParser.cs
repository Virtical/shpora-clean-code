using Markdown.Tokenizer.Tokens;
using Markdown.TokenParser.Nodes;

namespace Markdown.TokenParser.Parsers;

public class BodyParser : IParser
{
    public INode Match(TokenList tokens)
    {
        var (nodes, consumed) = MatchesStar.MatchStar(tokens, new ParagraphParser());

        if (nodes.Count == 0)
        {
            return new NullNode();
        }

        return new BodyNode(nodes, consumed);
    }
}