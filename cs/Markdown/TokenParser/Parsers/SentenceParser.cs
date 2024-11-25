using Markdown.Tokenizer.Tokens;
using Markdown.TokenParser.Nodes;

namespace Markdown.TokenParser.Parsers;

public class SentenceParser : IParser
{
    private readonly IParser[] parsers;
    public SentenceParser(params IParser[] parsers)
    {
        this.parsers = parsers;
    }
    public INode Match(TokenList tokens)
    {
        return MatchesFirst.MatchFirst(tokens, parsers);
    }
}