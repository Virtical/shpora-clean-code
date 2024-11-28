using System.Linq;
using Markdown.Tokenizer.Tokens;

namespace Markdown.Tokenizer.TokenScanners;

public class TextScanner : ITokenScanner
{
    private readonly EscapingScanner escapingScanner = new();
    private readonly SimpleScanner simpleScanner = new();
    private readonly NumberScanner numberScanner = new();
    public Token? TryGetToken(string plainMarkdown)
    {
        if (string.IsNullOrEmpty(plainMarkdown))
        {
            return null;
        }

        try
        {
            int currentIndex = 0;

            var text = new string(plainMarkdown
                .TakeWhile((c, index) =>
                {
                    var remainingText = plainMarkdown[currentIndex..];
                    
                    if (escapingScanner.TryGetToken(remainingText) != null)
                        return false;
                    if (simpleScanner.TryGetToken(remainingText) != null)
                        return false;
                    if (numberScanner.TryGetToken(remainingText) != null)
                        return false;
                    
                    currentIndex++;
                    return true;
                })
                .ToArray());

            return new Token(TypeOfToken.Word, text, currentIndex);
        }
        catch
        {
            return null;
        }
    }
}