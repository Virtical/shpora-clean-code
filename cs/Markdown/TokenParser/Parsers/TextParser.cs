using Markdown.Tokenizer.Tokens;
using Markdown.TokenParser.Nodes;

namespace Markdown.TokenParser.Parsers;

public class TextParser : IParser
{
    public INode Match(TokenList tokens)
    {
        return MatchesFirst.MatchFirst(tokens, new WordParser());
    }
}