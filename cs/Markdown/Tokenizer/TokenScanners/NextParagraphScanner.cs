using System.Collections.Generic;
using Markdown.Tokenizer.Tokens;

namespace Markdown.Tokenizer.TokenScanners;

public class NextParagraphScanner : ITokenScanner
{
    public Token? TryGetToken(string plainMarkdown)
    {
        if (string.IsNullOrEmpty(plainMarkdown))
        {
            return null;
        }
        
        return plainMarkdown.Length > 1 && plainMarkdown[0] == '\n' && plainMarkdown[1] == '\n' ? new Token(TypeOfToken.NextParagraph, 2) : null;
    }
}