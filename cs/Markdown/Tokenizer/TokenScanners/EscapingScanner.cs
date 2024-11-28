using System.Linq;
using Markdown.Tokenizer.Tokens;

namespace Markdown.Tokenizer.TokenScanners;

public class EscapingScanner : ITokenScanner
{
    public Token? TryGetToken(string plainMarkdown)
    {
        var escapedCharacters = new char[] { '\\', '_', '#' };
        if (string.IsNullOrEmpty(plainMarkdown))
        {
            return null;
        }

        if (plainMarkdown[0] != '\\') return null;
        
        if (plainMarkdown.Length > 1 && escapedCharacters.Contains(plainMarkdown[1]))
        {
            return new Token(TypeOfToken.Word, plainMarkdown[1].ToString(), 2);
        }

        return null;
    }
}