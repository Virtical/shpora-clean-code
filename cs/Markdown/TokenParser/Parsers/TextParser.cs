using Markdown.Tokenizer.Tokens;
using Markdown.TokenParser.Nodes;

namespace Markdown.TokenParser.Parsers;

public class TextParser : IParser
{
    public Node? TryMatch(TokenList tokens)
    {
        if (tokens.PeekOr(
                new[] { TypeOfToken.StartOfParagraph, TypeOfToken.Number, TypeOfToken.EndOfParagraph },
                new[] { TypeOfToken.StartOfParagraph, TypeOfToken.Word, TypeOfToken.EndOfParagraph },
                new[] { TypeOfToken.StartOfParagraph, TypeOfToken.Bullet, TypeOfToken.EndOfParagraph },
                new[] { TypeOfToken.StartOfParagraph, TypeOfToken.Underscore, TypeOfToken.EndOfParagraph },
                new[] { TypeOfToken.StartOfParagraph, TypeOfToken.Whitespace, TypeOfToken.EndOfParagraph }
            ))
        {
            return new Node(TypeOfNode.Text, tokens[1].Value, 3);
        }
        
        if (tokens.PeekOr(
                new[] { TypeOfToken.StartOfParagraph, TypeOfToken.Number },
                new[] { TypeOfToken.StartOfParagraph, TypeOfToken.Word },
                new[] { TypeOfToken.StartOfParagraph, TypeOfToken.Bullet },
                new[] { TypeOfToken.StartOfParagraph, TypeOfToken.Underscore },
                new[] { TypeOfToken.StartOfParagraph, TypeOfToken.Whitespace }
            ))
        {
            return new Node(TypeOfNode.Text, tokens[1].Value, 2);
        }
        
        if (tokens.PeekOr(
                new[] { TypeOfToken.Number, TypeOfToken.EndOfParagraph },
                new[] { TypeOfToken.Word, TypeOfToken.EndOfParagraph },
                new[] { TypeOfToken.Bullet, TypeOfToken.EndOfParagraph },
                new[] { TypeOfToken.Underscore, TypeOfToken.EndOfParagraph },
                new[] { TypeOfToken.Whitespace, TypeOfToken.EndOfParagraph }
            ))
        {
            return new Node(TypeOfNode.Text, tokens[0].Value, 2);
        }
        
        if (tokens.PeekOr(
                new[] { TypeOfToken.Number },
                new[] { TypeOfToken.Word },
                new[] { TypeOfToken.Bullet },
                new[] { TypeOfToken.Underscore },
                new[] { TypeOfToken.Whitespace }
            ))
        {
            return new Node(TypeOfNode.Text, tokens[0].Value, 1);
        }
        
        return null;
    }
}