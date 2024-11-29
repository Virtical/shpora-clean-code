using System.Linq;
using Markdown.Tokenizer.Tokens;
using Markdown.TokenParser.Nodes;

namespace Markdown.TokenParser.Parsers;

public class HashSentences : IParser
{
    public Node? TryMatch(TokenList tokens)
    {
        if (!tokens.Peek(TypeOfToken.StartOfParagraph, TypeOfToken.Hash, TypeOfToken.Whitespace))
        {
            return null;
        }

        const int additionalConsumed = 2;

        var newTokens = new TokenList(tokens.Take(1).Concat(tokens.Skip(3)));
        
        var (nodes, consumed) = MatchesStar.MatchStar(newTokens, new SentenceParser(new StrongParser(), new EmphasisParser(), new TextParser()));
        
        if (newTokens.PeekAt(consumed, TypeOfToken.NextParagraph))
        {
            consumed++;
        }

        return nodes.Count == 0 ? null : new Node(TypeOfNode.Heading, nodes, consumed + additionalConsumed);
    }
}