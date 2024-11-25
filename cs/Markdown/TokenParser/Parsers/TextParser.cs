using Markdown.Tokenizer.Tokens;
using Markdown.TokenParser.Nodes;

namespace Markdown.TokenParser.Parsers;

public class TextParser : IParser
{
    public INode Match(TokenList tokens)
    {
        if (tokens.PeekOr(
                new[] { TypeOfToken.StartOfFile, TypeOfToken.Number, TypeOfToken.EndOfFile },
                new[] { TypeOfToken.StartOfFile, TypeOfToken.Word, TypeOfToken.EndOfFile },
                new[] { TypeOfToken.StartOfFile, TypeOfToken.Underscore, TypeOfToken.EndOfFile },
                new[] { TypeOfToken.StartOfFile, TypeOfToken.Whitespace, TypeOfToken.EndOfFile }
            ))
        {
            return new Node(TypeOfNode.Text, tokens[1].Value, 3);
        }
        
        if (tokens.PeekOr(
                new[] { TypeOfToken.StartOfFile, TypeOfToken.Number },
                new[] { TypeOfToken.StartOfFile, TypeOfToken.Word },
                new[] { TypeOfToken.StartOfFile, TypeOfToken.Underscore },
                new[] { TypeOfToken.StartOfFile, TypeOfToken.Whitespace }
            ))
        {
            return new Node(TypeOfNode.Text, tokens[1].Value, 2);
        }
        
        if (tokens.PeekOr(
                new[] { TypeOfToken.Number, TypeOfToken.EndOfFile },
                new[] { TypeOfToken.Word, TypeOfToken.EndOfFile },
                new[] { TypeOfToken.Underscore, TypeOfToken.EndOfFile },
                new[] { TypeOfToken.Whitespace, TypeOfToken.EndOfFile }
            ))
        {
            return new Node(TypeOfNode.Text, tokens[0].Value, 2);
        }
        
        if (tokens.PeekOr(
                new[] { TypeOfToken.Number },
                new[] { TypeOfToken.Word },
                new[] { TypeOfToken.Underscore },
                new[] { TypeOfToken.Whitespace }
            ))
        {
            return new Node(TypeOfNode.Text, tokens[0].Value, 1);
        }
        
        return new NullNode();
    }
}