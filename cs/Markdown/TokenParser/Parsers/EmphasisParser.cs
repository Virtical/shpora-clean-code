using System.Collections.Generic;
using Markdown.Tokenizer.Tokens;
using Markdown.TokenParser.Nodes;

namespace Markdown.TokenParser.Parsers;

public class EmphasisParser : IParser
{
    public INode Match(TokenList tokens)
    {
        if (tokens.Peek(TypeOfToken.StartOfFile, TypeOfToken.Underscore, TypeOfToken.Word, TypeOfToken.Underscore, TypeOfToken.EndOfFile))
        {
            return new EmphasisNode(TypeOfNode.Emphasis, new List<INode>{ new Node(TypeOfNode.Text, tokens[2].Value, 1) }, 5);
        }
        
        if (tokens.Peek(TypeOfToken.StartOfFile, TypeOfToken.Underscore, TypeOfToken.Word, TypeOfToken.Underscore))
        {
            return new EmphasisNode(TypeOfNode.Emphasis, new List<INode>{ new Node(TypeOfNode.Text, tokens[2].Value, 1) }, 4);
        }
        
        if (tokens.Peek(TypeOfToken.Underscore, TypeOfToken.Word, TypeOfToken.Underscore, TypeOfToken.EndOfFile))
        {
            return new EmphasisNode(TypeOfNode.Emphasis, new List<INode>{ new Node(TypeOfNode.Text, tokens[1].Value, 1) }, 4);
        }
        
        if (tokens.Peek(TypeOfToken.Underscore, TypeOfToken.Word, TypeOfToken.Underscore))
        {
            return new EmphasisNode(TypeOfNode.Emphasis, new List<INode>{ new Node(TypeOfNode.Text, tokens[1].Value, 1) }, 3);
        }
        
        if (!tokens.PeekOr(
                new[] { TypeOfToken.StartOfFile, TypeOfToken.Underscore, TypeOfToken.Number },
                new[] { TypeOfToken.StartOfFile, TypeOfToken.Underscore, TypeOfToken.Word },
                new[] { TypeOfToken.Whitespace, TypeOfToken.Underscore, TypeOfToken.Number },
                new[] { TypeOfToken.Whitespace, TypeOfToken.Underscore, TypeOfToken.Word }
            ))
        {
            return new NullNode();
        }
        
        var newTokens = tokens.Grab(2);
        
        var (nodes, consumed) = MatchesStar.MatchStar(newTokens, new EmphasisCloseParser());
        
        if (newTokens.PeekAtOr(consumed, 
                new[] { TypeOfToken.Number, TypeOfToken.Underscore, TypeOfToken.EndOfFile },
                new[] { TypeOfToken.Word, TypeOfToken.Underscore, TypeOfToken.EndOfFile }
            ))
        {
            nodes.Add(new Node(TypeOfNode.Text, newTokens.Offset(consumed)[0].Value, 1));
        
            consumed += 5;
            
            return new EmphasisNode(TypeOfNode.Emphasis, nodes, consumed);
        }
        
        if (newTokens.PeekAtOr(consumed, 
                new[] { TypeOfToken.Number, TypeOfToken.Underscore, TypeOfToken.Whitespace },
                new[] { TypeOfToken.Word, TypeOfToken.Underscore, TypeOfToken.Whitespace }
            ))
        {
            nodes.Add(new Node(TypeOfNode.Text, newTokens.Offset(consumed)[0].Value, 1));
        
            consumed += 4;
            
            return new EmphasisNode(TypeOfNode.Emphasis, nodes, consumed);
        }

        return new NullNode();

    }
}