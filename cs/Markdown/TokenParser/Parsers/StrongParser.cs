using Markdown.Tokenizer.Tokens;
using Markdown.TokenParser.Nodes;

namespace Markdown.TokenParser.Parsers;

public class StrongParser : IParser
{
    public INode Match(TokenList tokens)
    {
        if (!tokens.PeekOr(
                new[] { TypeOfToken.StartOfFile, TypeOfToken.Underscore, TypeOfToken.Underscore, TypeOfToken.Number },
                new[] { TypeOfToken.StartOfFile, TypeOfToken.Underscore, TypeOfToken.Underscore, TypeOfToken.Word },
                new[] { TypeOfToken.Whitespace, TypeOfToken.Underscore, TypeOfToken.Underscore, TypeOfToken.Number },
                new[] { TypeOfToken.Whitespace, TypeOfToken.Underscore, TypeOfToken.Underscore, TypeOfToken.Word }
                ))
        {
            return new NullNode();
        }
        
        var newTokens = tokens.Offset(3);
        
        var (nodes, consumed) = MatchesStar.MatchStar(newTokens, new StrongCloseParser());
        
        if (tokens.PeekOr(
                new[] { TypeOfToken.Whitespace },
                new[] { TypeOfToken.Whitespace }
            ))
        {
            nodes.Insert(0, new Node(TypeOfNode.Text, " ", 1));
        }
        
        if (newTokens.PeekAtOr(consumed, 
                new[] { TypeOfToken.Number, TypeOfToken.Underscore, TypeOfToken.Underscore, TypeOfToken.EndOfFile },
                new[] { TypeOfToken.Word, TypeOfToken.Underscore, TypeOfToken.Underscore, TypeOfToken.EndOfFile }
                ))
        {
            nodes.Add(new Node(TypeOfNode.Text, newTokens.Offset(consumed)[0].Value, 1));
        
            consumed += 7;
        
            return new StrongNode(TypeOfNode.Strong, nodes, consumed);
        }
        
        if (newTokens.PeekAtOr(consumed, 
                new[] { TypeOfToken.Number, TypeOfToken.Underscore, TypeOfToken.Underscore, TypeOfToken.Whitespace },
                new[] { TypeOfToken.Word, TypeOfToken.Underscore, TypeOfToken.Underscore, TypeOfToken.Whitespace }
            ))
        {
            nodes.Add(new Node(TypeOfNode.Text, newTokens.Offset(consumed)[0].Value, 1));
        
            consumed += 6;
        
            return new StrongNode(TypeOfNode.Strong, nodes, consumed);
        }

        return new NullNode();

    }
}