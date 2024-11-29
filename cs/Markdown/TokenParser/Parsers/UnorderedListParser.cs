using System.Linq;
using Markdown.Tokenizer.Tokens;
using Markdown.TokenParser.Nodes;

namespace Markdown.TokenParser.Parsers;

public class UnorderedListParser : IParser
{
    private readonly int nestedLevel;
    
    public UnorderedListParser(int nestedLevel = 0)
    {
        this.nestedLevel = nestedLevel;
    }
    public Node? TryMatch(TokenList tokens)
    {
        var (nodes, consumed) = MatchesStar.MatchStar(tokens, new ListItemParser(nestedLevel));
        
        return nodes.Count == 0 ? null : new Node(TypeOfNode.UnorderedList, nodes, consumed);
    }
}