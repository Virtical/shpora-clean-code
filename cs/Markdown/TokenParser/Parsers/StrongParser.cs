using System.Collections.Generic;
using Markdown.Tokenizer.Tokens;
using Markdown.TokenParser.Nodes;

namespace Markdown.TokenParser.Parsers;

public class StrongParser : IParser
{
    public Node? TryMatch(TokenList tokens)
    {
        var openingNodes = TryMatchOpeningStrong(tokens);
        if (openingNodes == null)
        {
            return null;
        }
        
        var newTokens = tokens.Offset(3);
        
        var (contentNodes, consumed) = MatchesStar.MatchStar(newTokens, new StrongCloseParser());
        openingNodes.AddRange(contentNodes);
        
        return TryMatchClosingStrong(newTokens, openingNodes, consumed);
    }

    private static List<Node>? TryMatchOpeningStrong(TokenList tokens)
    {
        if (tokens.PeekOr(
                new[] { TypeOfToken.StartOfFile, TypeOfToken.Underscore, TypeOfToken.Underscore, TypeOfToken.Number },
                new[] { TypeOfToken.StartOfFile, TypeOfToken.Underscore, TypeOfToken.Underscore, TypeOfToken.Word }))
        {
            return new List<Node>();
        }

        if (tokens.PeekOr(
                new[] { TypeOfToken.Whitespace, TypeOfToken.Underscore, TypeOfToken.Underscore, TypeOfToken.Number },
                new[] { TypeOfToken.Whitespace, TypeOfToken.Underscore, TypeOfToken.Underscore, TypeOfToken.Word }))
        {
            return new List<Node> { new Node(TypeOfNode.Text, " ", 1) };
        }

        return null;
    }

    private static Node? TryMatchClosingStrong(TokenList tokens, List<Node> nodes, int consumed)
    {
        var additionalConsumed = 0;

        if (tokens.PeekAtOr(consumed,
                new[] { TypeOfToken.Number, TypeOfToken.Underscore, TypeOfToken.Underscore, TypeOfToken.EndOfFile },
                new[] { TypeOfToken.Word, TypeOfToken.Underscore, TypeOfToken.Underscore, TypeOfToken.EndOfFile }))
        {
            additionalConsumed = 7;
        }
        else if (tokens.PeekAtOr(consumed,
                 new[] { TypeOfToken.Number, TypeOfToken.Underscore, TypeOfToken.Underscore, TypeOfToken.Whitespace },
                 new[] { TypeOfToken.Word, TypeOfToken.Underscore, TypeOfToken.Underscore, TypeOfToken.Whitespace }))
        {
            additionalConsumed = 6;
        }
        else
        {
            return null;
        }
        
        nodes.Add(new Node(TypeOfNode.Text, tokens.Offset(consumed)[0].Value, 1));
        return new Node(TypeOfNode.Strong, nodes, consumed + additionalConsumed);
    }
}