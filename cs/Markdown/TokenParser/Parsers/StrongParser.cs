﻿using Markdown.Tokenizer.Tokens;
using Markdown.TokenParser.Nodes;

namespace Markdown.TokenParser.Parsers;

public class StrongParser : IParser
{
    public INode Match(TokenList tokens)
    {
        if (!tokens.PeekOr(
                new[] { TypeOfToken.StartOfFile, TypeOfToken.Underscore, TypeOfToken.Underscore, TypeOfToken.Word },
                new[] { TypeOfToken.Whitespace, TypeOfToken.Underscore, TypeOfToken.Underscore, TypeOfToken.Word }
                ))
        {
            return new NullNode();
        }
        
        var newTokens = tokens.Offset(3);
        
        var (nodes, consumed) = MatchesStar.MatchStar(newTokens, new StrongCloseParser());
        
        if (!newTokens.PeekAtOr(consumed, 
                new[] { TypeOfToken.Word, TypeOfToken.Underscore, TypeOfToken.Underscore, TypeOfToken.EndOfFile },
                new[] { TypeOfToken.Word, TypeOfToken.Underscore, TypeOfToken.Underscore, TypeOfToken.Whitespace }
                ))
        {
            return new NullNode();
        }
        
        nodes.Add(new Node(TypeOfNode.Text, newTokens.Offset(consumed).First.Value, 1));
        
        consumed += 7;
        
        return new StrongNode(TypeOfNode.Strong, nodes, consumed);
    }
}