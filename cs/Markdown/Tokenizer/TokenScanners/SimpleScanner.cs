using System.Collections.Generic;
using Markdown.Tokenizer.Tokens;

namespace Markdown.Tokenizer.TokenScanners;

public class SimpleScanner : ITokenScanner
{
    private static readonly Dictionary<char, TypeOfToken> TokenTypes = new Dictionary<char, TypeOfToken>
    {
        { '_', TypeOfToken.Underscore },
        { '\n', TypeOfToken.Newline },
        { ' ', TypeOfToken.Whitespace },
        { '#', TypeOfToken.Hash }
    };

    public Token? FromString(string plainMarkdown)
    {
        if (string.IsNullOrEmpty(plainMarkdown))
        {
            return null;
        }

        var firstChar = plainMarkdown[0];
        return TokenTypes.TryGetValue(firstChar, out var tokenType) ? new Token(tokenType, firstChar.ToString()) : null;
    }
}