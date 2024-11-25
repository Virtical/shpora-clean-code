using System.Linq;
using Markdown.Tokenizer.Tokens;
using Markdown.TokenParser.Nodes;

namespace Markdown.TokenParser.Parsers;

public class StrongCloseParser : IParser
{
    public INode Match(TokenList tokens)
    {
        if (tokens.PeekOr(
                new[] { TypeOfToken.Number, TypeOfToken.Underscore, TypeOfToken.Underscore, TypeOfToken.Whitespace },
                new[] { TypeOfToken.Word, TypeOfToken.Underscore, TypeOfToken.Underscore, TypeOfToken.Whitespace },
                new[] { TypeOfToken.Number, TypeOfToken.Underscore, TypeOfToken.Underscore, TypeOfToken.EndOfFile },
                new[] { TypeOfToken.Word, TypeOfToken.Underscore, TypeOfToken.Underscore, TypeOfToken.EndOfFile }
                ))
        {
            return new NullNode();
        }

        var node = MatchesFirst.MatchFirst(tokens, new EmphasisParser());
        if (!node.Null)
        {
            return node;
        }

        return tokens.Any() ? new Node(TypeOfNode.Text, tokens[0].Value, 1) : new NullNode();
    }
}