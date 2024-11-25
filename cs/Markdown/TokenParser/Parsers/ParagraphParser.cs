using Markdown.Tokenizer.Tokens;
using Markdown.TokenParser.Nodes;

namespace Markdown.TokenParser.Parsers;

public class ParagraphParser : IParser
{
    public INode Match(TokenList tokens)
    {
        return MatchesFirst.MatchFirst(tokens, new HashSentencesAndEofParser(), new SentencesAndEofParser());
    }
}