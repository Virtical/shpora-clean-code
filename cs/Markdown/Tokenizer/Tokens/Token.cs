namespace Markdown.Tokenizer.Tokens;
public class Token
{
    public TypeOfToken Type { get; }
    public string? Value { get; }
    public int Length => Value.Length;

    public Token(TypeOfToken type) 
        : this(type, "")
    {
    }
    
    public Token(TypeOfToken type, string? value)
    {
        Type = type;
        Value = value;
    }
}
