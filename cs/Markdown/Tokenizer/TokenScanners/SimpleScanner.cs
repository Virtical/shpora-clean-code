using System.Collections.Generic;
using Markdown.Tokenizer.Tokens;

namespace Markdown.Tokenizer.TokenScanners;

public class SimpleScanner : ITokenScanner
{
    private static readonly Dictionary<char, TypeOfToken> TokenTypes = new Dictionary<char, TypeOfToken>
    {
        { '_', TypeOfToken.Underscore },
        { ' ', TypeOfToken.Whitespace },
        { '\n', TypeOfToken.Newline },
        { '#', TypeOfToken.Hash },
        { '-', TypeOfToken.Bullet },
        { '+', TypeOfToken.Bullet },
        { '*', TypeOfToken.Bullet }
    };

    public Token? TryGetToken(string plainMarkdown)
    {
        if (string.IsNullOrEmpty(plainMarkdown))
        {
            return null;
        }

        var firstChar = plainMarkdown[0];
        return TokenTypes.TryGetValue(firstChar, out var tokenType) ? new Token(tokenType, firstChar.ToString()) : null;
    }
}