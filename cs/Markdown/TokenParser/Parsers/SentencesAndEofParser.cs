using Markdown.Tokenizer.Tokens;
using Markdown.TokenParser.Nodes;

namespace Markdown.TokenParser.Parsers;

public class SentencesAndEofParser : IParser
{
    public INode Match(TokenList tokens)
    {
        var (nodes, consumed) = MatchesStar.MatchStar(tokens, new SentenceParser(new StrongParser(), new EmphasisParser(), new TextParser()));

        if (nodes.Count == 0)
        {
            return new NullNode();
        }

        return new ParagraphNode(nodes, consumed);
    }
}