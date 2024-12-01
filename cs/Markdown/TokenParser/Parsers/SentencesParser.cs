using Markdown.Tokenizer.Tokens;
using Markdown.TokenParser.Nodes;

namespace Markdown.TokenParser.Parsers;

public class SentencesParser : IParser
{
    public Node? TryMatch(TokenList tokens)
    {
        var sentenceParser = CreateSentenceParser();
        var (nodes, consumed) = MatchesStar.MatchStar(tokens, sentenceParser);

        if (tokens.PeekAt(consumed, TypeOfToken.Newline))
        {
            consumed++;
        }

        return nodes.Count == 0 ? null : new Node(TypeOfNode.Paragraph, nodes, consumed);
    }
    
    private static SentenceParser CreateSentenceParser()
    {
        return new SentenceParser(
            new UnorderedListParser(), 
            new IntersectionParser(),
            new StrongParser(), 
            new EmphasisParser(), 
            new TextParser());
    }
}