using System.Linq;
using Markdown.Tokenizer.Tokens;

namespace Markdown.Tokenizer.TokenScanners;

public class TextScanner : ITokenScanner
{
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
            var text = new string(plainMarkdown
                .TakeWhile(c => 
                    simpleScanner.TryGetToken(c.ToString()) == null &&
                    numberScanner.TryGetToken(c.ToString()) == null)
                .ToArray());

            return new Token(TypeOfToken.Word, text);
        }
        catch
        {
            return null;
        }
    }
}