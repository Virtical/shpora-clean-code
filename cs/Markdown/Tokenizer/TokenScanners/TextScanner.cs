using System.Linq;
using Markdown.Tokenizer.Tokens;

namespace Markdown.Tokenizer.TokenScanners;

public class TextScanner : ITokenScanner
{
    private readonly SimpleScanner simpleScanner = new();
    public Token? FromString(string plainMarkdown)
    {
        if (string.IsNullOrEmpty(plainMarkdown))
        {
            return null;
        }

        try
        {
            var text = new string(plainMarkdown
                .TakeWhile(с => simpleScanner.FromString(с.ToString()) == null)
                .ToArray());

            return new Token(TypeOfToken.Word, text);
        }
        catch
        {
            return null;
        }
    }
}