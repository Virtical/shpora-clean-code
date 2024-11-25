using Markdown.Tokenizer.Tokens;
using Markdown.TokenParser.Nodes;

namespace Markdown.TokenParser.Parsers;

public class HashSentencesAndEofParser : IParser
{
    public INode Match(TokenList tokens)
    {
        if (!tokens.Peek(TypeOfToken.StartOfFile, TypeOfToken.Hash, TypeOfToken.Whitespace))
        {
            return new NullNode();
        }

        var newTokens = tokens.Offset(3);
        
        var (nodes, consumed) = MatchesStar.MatchStar(newTokens, new SentenceParser(new StrongParser(), new EmphasisParser(), new TextParser()));

        if (nodes.Count == 0)
        {
            return new NullNode();
        }
        
        consumed += 3;

        return new HeadingNode(nodes, consumed);
    }
}