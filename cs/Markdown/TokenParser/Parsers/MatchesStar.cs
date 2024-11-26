﻿using System.Collections.Generic;
using Markdown.Tokenizer.Tokens;
using Markdown.TokenParser.Nodes;

namespace Markdown.TokenParser.Parsers;

public class MatchesStar
{
    public static (List<INode> MatchedNodes, int Consumed) MatchStar(TokenList tokens, IParser parser)
    {
        var matchedNodes = new List<INode>();
        var consumed = 0;

        while (true)
        {
            var node = parser.Match(tokens.Offset(consumed));
            if (node.Null)
                break;

            if (node.Type is TypeOfNode.Emphasis or TypeOfNode.Strong)
            {
                if (node.Descendants[0].Value == " ")
                {
                    matchedNodes.Add(node.Descendants[0]);
                    node.Descendants.RemoveAt(0);
                }
            }
            
            matchedNodes.Add(node);
            consumed += node.Consumed;
        }

        return (matchedNodes, consumed);
    }
}