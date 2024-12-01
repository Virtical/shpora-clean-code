using System.Linq;
using Markdown.Tokenizer.Tokens;
using Markdown.TokenParser.Nodes;

namespace Markdown.TokenParser.Parsers;

public class HashSentences : IParser
{
    public Node? TryMatch(TokenList tokens)
    {
        if (!TryExtractHeadingTokens(tokens, out var remainingTokens) || remainingTokens == null)
        {
            return null;
        }

        var sentenceParser = CreateSentenceParser();
        
        var (nodes, consumed) = MatchesStar.MatchStar(remainingTokens, sentenceParser);
        
        if (remainingTokens.PeekAt(consumed, TypeOfToken.Newline))
        {
            consumed++;
        }

        return nodes.Count == 0 ? null : new Node(TypeOfNode.Heading, nodes, consumed + 2);
    }

    private static bool TryExtractHeadingTokens(TokenList tokens, out TokenList? remainingTokens)
    {
        remainingTokens = default;
        
        if (!tokens.Peek(TypeOfToken.StartOfParagraph, TypeOfToken.Hash, TypeOfToken.Whitespace))
        {
            return false;
        }

        remainingTokens = new TokenList(tokens.Take(1).Concat(tokens.Skip(3)));
        return true;
    }
    
    private static SentenceParser CreateSentenceParser()
    {
        return new SentenceParser(
            new UnorderedListParser(),
            new StrongParser(),
            new EmphasisParser(),
            new TextParser());
    }
}