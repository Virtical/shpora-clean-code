using Markdown.Tokenizer.Tokens;

namespace Markdown.Tokenizer.TokenScanners;

public interface ITokenScanner
{
    Token? FromString(string input);
}