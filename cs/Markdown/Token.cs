using System.Collections.Generic;

namespace Markdown;
public class Token
{
    public TypeOfToken Type { get; }
    public string Content { get; }
    public List<Token> Children { get; }

    public Token(TypeOfToken type, string content, List<Token> children = null)
    {
        Type = type;
        Content = content;
        Children = children ?? new List<Token>();
    }
}
