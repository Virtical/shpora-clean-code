using Markdown.Tokenizer.Tokens;

namespace Markdown.Tokenizer.TokenScanners;

public class NewlineScanner : ITokenScanner
{
    public bool TryGetToken(string plainMarkdown, int index, out Token? token)
    {
        token = default;

        if (plainMarkdown[index] != '\n')
        {
            return false;
        }

        if (plainMarkdown.Length > index + 1 && plainMarkdown[index + 1] == '\r')
        {
            token = new Token(TypeOfToken.Newline, "\n\r");
            return true;
        }

        token = new Token(TypeOfToken.Newline, "\n");
        return true;

    }
}