using System.Collections.Generic;
using System.Linq;
using Markdown.Tokenizer.Tokens;
using Markdown.TokenParser.Nodes;

namespace Markdown.TokenParser.Parsers;

public class ListItemParser : IParser
{
    private readonly int nestedLevel;

    public ListItemParser(int nestedLevel)
    {
        this.nestedLevel = nestedLevel;
    }
    public Node? TryMatch(TokenList tokens)
    {
        var openingNodes = IsOpeningListItem(tokens, nestedLevel);
        if (openingNodes == false)
        {
            return null;
        }
        
        var newTokens = tokens.Offset(3 + nestedLevel * 4);

        var (nodes, consumed) = MatchesStar.MatchStar(newTokens, new SentenceParser(new UnorderedListParser(nestedLevel+1), new StrongParser(), new EmphasisParser(), new TextParser()));

        if (nodes.Count == 0)
        {
            return null;
        }
        
        consumed += 3 + nestedLevel * 4;
        
        return new Node(TypeOfNode.ListItem, nodes, consumed);
    }
    
    private static bool IsOpeningListItem(TokenList tokens, int nestedLevel)
    {
        if (!tokens.PeekAt(1, Enumerable.Repeat(TypeOfToken.Whitespace, nestedLevel * 4).ToArray()))
        {
            return false;
        }
        
        var newTokens = new TokenList(tokens.Take(1).Concat(tokens.Skip(nestedLevel * 4 + 1)));
        
        if (newTokens.PeekOr(
                new[] { TypeOfToken.StartOfParagraph, TypeOfToken.Bullet, TypeOfToken.Whitespace },
                new[] { TypeOfToken.Newline, TypeOfToken.Bullet, TypeOfToken.Whitespace }))
        {
            return true;
        }

        return false;
    }
}