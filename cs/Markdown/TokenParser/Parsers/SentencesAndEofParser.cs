using Markdown.Tokenizer.Tokens;
using Markdown.TokenParser.Nodes;

namespace Markdown.TokenParser.Parsers;

public class SentencesAndEofParser : IParser
{
    public Node? TryMatch(TokenList tokens)
    {
        var (nodes, consumed) = MatchesStar.MatchStar(tokens, new SentenceParser(new StrongParser(), new EmphasisParser(), new TextParser()));

        return nodes.Count == 0 ? null : new Node(TypeOfNode.Paragraph, nodes, consumed);
    }
}