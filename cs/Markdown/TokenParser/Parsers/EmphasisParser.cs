using Markdown.Tokenizer.Tokens;
using Markdown.TokenParser.Nodes;

namespace Markdown.TokenParser.Parsers;

public class EmphasisParser : IParser
{
    public INode Match(TokenList tokens)
    {
        if (!tokens.PeekOr(
                new[] { TypeOfToken.StartOfFile, TypeOfToken.Underscore, TypeOfToken.Word },
                new[] { TypeOfToken.Whitespace, TypeOfToken.Underscore, TypeOfToken.Word }
            ))
        {
            return new NullNode();
        }
        
        var newTokens = tokens.Grab(2);
        
        var (nodes, consumed) = MatchesStar.MatchStar(newTokens, new EmphasisCloseParser());
        
        if (!newTokens.PeekAtOr(consumed, 
                new[] { TypeOfToken.Word, TypeOfToken.Underscore, TypeOfToken.EndOfFile },
                new[] { TypeOfToken.Word, TypeOfToken.Underscore, TypeOfToken.Whitespace }
            ))
        {
            return new NullNode();
        }
        
        nodes.Add(new Node(TypeOfNode.Text, newTokens.Offset(consumed).First.Value, 1));
        
        consumed += 5;
        
        return new EmphasisNode(TypeOfNode.Emphasis, nodes, consumed);
    }
}