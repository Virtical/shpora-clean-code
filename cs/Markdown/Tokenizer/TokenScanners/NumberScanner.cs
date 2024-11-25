using System.Linq;
using Markdown.Tokenizer.Tokens;

namespace Markdown.Tokenizer.TokenScanners;

public class NumberScanner : ITokenScanner
{
    public Token? FromString(string plainMarkdown)
    {
        if (string.IsNullOrEmpty(plainMarkdown))
        {
            return null;
        }
        
        var number = new string(plainMarkdown
            .TakeWhile(char.IsDigit)
            .ToArray());
            
        return number.Length == 0 ? null : new Token(TypeOfToken.Number, number);
    }
}