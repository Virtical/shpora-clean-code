using Markdown.Tokenizer.Tokens;
using Markdown.TokenParser.Nodes;

namespace Markdown.TokenParser.Parsers;

public class MatchesFirst
{
    public static INode MatchFirst(TokenList tokens, params IParser[] parsers)
    {
        foreach (var parser in parsers)
        {
            var node = parser.Match(tokens);
            if (node.Present)
            {
                return node;
            }
        }

        return new NullNode();
    }
}