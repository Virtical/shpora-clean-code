using System.Linq;
using Markdown.Tokenizer.Tokens;
using Markdown.TokenParser.Nodes;

namespace Markdown.TokenParser.Parsers;

public class StrongCloseParser : IParser
{
    public Node? TryMatch(TokenList tokens)
    {
        if (tokens.PeekOr(
                new[] { TypeOfToken.Number, TypeOfToken.Underscore, TypeOfToken.Underscore, TypeOfToken.Whitespace },
                new[] { TypeOfToken.Word, TypeOfToken.Underscore, TypeOfToken.Underscore, TypeOfToken.Whitespace },
                new[] { TypeOfToken.Number, TypeOfToken.Underscore, TypeOfToken.Underscore, TypeOfToken.EndOfParagraph },
                new[] { TypeOfToken.Word, TypeOfToken.Underscore, TypeOfToken.Underscore, TypeOfToken.EndOfParagraph }
                ))
        {
            return null;
        }

        var node = MatchesFirst.MatchFirst(tokens, new EmphasisParser());
        if (node != null)
        {
            return node;
        }

        return tokens.Any() && tokens[0].Type != TypeOfToken.EndOfParagraph ? new Node(TypeOfNode.Text, tokens[0].Value, 1) : null;
    }
}