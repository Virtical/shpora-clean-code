using Markdown.Tokenizer.Tokens;
using Markdown.TokenParser.Nodes;

namespace Markdown.TokenParser.Parsers;

public class NewlineParser : IParser
{
    public Node? TryMatch(TokenList tokens)
    {
        if (tokens.Peek(TypeOfToken.StartOfParagraph, TypeOfToken.Newline, TypeOfToken.EndOfParagraph))
        {
            return new Node(TypeOfNode.Text, tokens[1].Value, 3);
        }
        
        if (tokens.Peek(TypeOfToken.StartOfParagraph, TypeOfToken.Newline))
        {
            return new Node(TypeOfNode.Text, tokens[1].Value, 2);
        }
        
        if (tokens.Peek(TypeOfToken.Newline, TypeOfToken.EndOfParagraph))
        {
            return new Node(TypeOfNode.Text, tokens[0].Value, 2);
        }
        
        if (tokens.Peek(TypeOfToken.Newline))
        {
            return new Node(TypeOfNode.Text, tokens[0].Value, 1);
        }
        
        return null;
    }
}