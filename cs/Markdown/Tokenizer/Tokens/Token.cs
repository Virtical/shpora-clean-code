namespace Markdown.Tokenizer.Tokens;

public class Token
{
    public TypeOfToken Type { get; }
    public string Value { get; }
    public int Length { get; }

    public Token(TypeOfToken type) 
        : this(type, "")
    {
    }
    
    public Token(TypeOfToken type, string value, int length)
    {
        Type = type;
        Value = value;
        Length = length;
    }
        
    public Token(TypeOfToken type, string value)
    {
        Type = type;
        Value = value;
        Length = Value.Length;
    }
    
    public Token(TypeOfToken type, int length)
    {
        Type = type;
        Value = "";
        Length = length;
    }
}