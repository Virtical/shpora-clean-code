using Markdown.Tokenizer.Tokens;

namespace Markdown.Tokenizer.TokenScanners;

public interface ITokenScanner
{
    Token? TryGetToken(string input);
}