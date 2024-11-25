﻿using System.Linq;
using Markdown.Tokenizer.Tokens;
using Markdown.TokenParser.Nodes;

namespace Markdown.TokenParser.Parsers;

public class EmphasisCloseParser : IParser
{
    public INode Match(TokenList tokens)
    {
        if (tokens.PeekOr(
                new[] {TypeOfToken.Word, TypeOfToken.Underscore, TypeOfToken.Whitespace},
                new[] {TypeOfToken.Word, TypeOfToken.Underscore, TypeOfToken.EndOfFile}
            ))
        {
            return new NullNode();
        }

        return tokens.Any() ? new Node(TypeOfNode.Text, tokens.First.Value, 1) : new NullNode();
    }
}