using System.Collections.Generic;
using Markdown.Tokenizer.Tokens;
using Markdown.TokenParser.Nodes;

namespace Markdown.TokenParser.Parsers;

public class EmphasisParser : IParser
{
    public Node? TryMatch(TokenList tokens)
    {
        var node = TryMatchPartOfWordEmphasis(tokens);
        if (node != null)
        {
            return node;
        }
        
        var openingNodes = TryMatchOpeningEmphasis(tokens);
        if (openingNodes == null)
        {
            return null;
        }
        
        var newTokens = tokens.Offset(2);
        
        var (contentNodes, consumed) = MatchesStar.MatchStar(newTokens, new EmphasisCloseParser());
        openingNodes.AddRange(contentNodes);
        
        return TryMatchClosingEmphasis(newTokens, openingNodes, consumed);
    }

    private static Node? TryMatchPartOfWordEmphasis(TokenList tokens)
    {
        if (tokens.Peek(TypeOfToken.StartOfParagraph, TypeOfToken.Underscore, TypeOfToken.Word, TypeOfToken.Underscore, TypeOfToken.EndOfParagraph))
        {
            return new Node(TypeOfNode.Emphasis, new List<Node>{ new Node(TypeOfNode.Text, tokens[2].Value, 1) }, 5);
        }

        if (tokens.Peek(TypeOfToken.StartOfParagraph, TypeOfToken.Underscore, TypeOfToken.Word, TypeOfToken.Underscore))
        {
            return new Node(TypeOfNode.Emphasis, new List<Node>{ new Node(TypeOfNode.Text, tokens[2].Value, 1) }, 4);
        }

        if (tokens.Peek(TypeOfToken.Underscore, TypeOfToken.Word, TypeOfToken.Underscore, TypeOfToken.EndOfParagraph))
        {
            return new Node(TypeOfNode.Emphasis, new List<Node>{ new Node(TypeOfNode.Text, tokens[1].Value, 1) }, 4);
        }
        
        if (tokens.Peek(TypeOfToken.Underscore, TypeOfToken.Word, TypeOfToken.Underscore))
        {
            return new Node(TypeOfNode.Emphasis, new List<Node>{ new Node(TypeOfNode.Text, tokens[1].Value, 1) }, 3);
        }

        return null;
    }

    private static List<Node>? TryMatchOpeningEmphasis(TokenList tokens)
    {
        if (tokens.PeekOr(
                new[] { TypeOfToken.StartOfParagraph, TypeOfToken.Underscore, TypeOfToken.Number },
                new[] { TypeOfToken.StartOfParagraph, TypeOfToken.Underscore, TypeOfToken.Word }))
        {
            return new List<Node>();
        }

        if (tokens.PeekOr(
                new[] { TypeOfToken.Whitespace, TypeOfToken.Underscore, TypeOfToken.Number },
                new[] { TypeOfToken.Whitespace, TypeOfToken.Underscore, TypeOfToken.Word }))
        {
            return new List<Node> { new Node(TypeOfNode.Text, " ", 1) };
        }

        return null;
    }

    private static Node? TryMatchClosingEmphasis(TokenList tokens, List<Node> nodes, int consumed)
    {
        var additionalConsumed = 0;

        if (tokens.PeekAtOr(consumed,
                new[] { TypeOfToken.Number, TypeOfToken.Underscore, TypeOfToken.EndOfParagraph },
                new[] { TypeOfToken.Word, TypeOfToken.Underscore, TypeOfToken.EndOfParagraph }))
        {
            additionalConsumed = 5;
        }
        else if (tokens.PeekAtOr(consumed,
                 new[] { TypeOfToken.Number, TypeOfToken.Underscore, TypeOfToken.Whitespace },
                 new[] { TypeOfToken.Word, TypeOfToken.Underscore, TypeOfToken.Whitespace }))
        {
            additionalConsumed = 4;
        }
        else
        {
            return null;
        }
        
        nodes.Add(new Node(TypeOfNode.Text, tokens.Offset(consumed)[0].Value, 1));
        return new Node(TypeOfNode.Emphasis, nodes, consumed + additionalConsumed);
    }
}