using System;
using System.Linq;
using Markdown.Tokenizer.Tokens;
using Markdown.TokenParser.Nodes;
using Markdown.TokenParser.Parsers;

namespace Markdown.TokenParser;

public class MdTokenParser : IMdTokenParser
{
    public INode Parse(TokenList tokens)
    {
        var body = new BodyParser().Match(tokens);

        if (tokens.Count() != body.Consumed)
        {
            throw new Exception($"Syntax error: {tokens[body.Consumed]}");
        }

        return body;
    }
}