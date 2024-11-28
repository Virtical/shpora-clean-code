﻿using System.Linq;
using Markdown.TokenParser.Nodes;

namespace Markdown.Converter.Visitors;

public class EmphasisVisitor : IVisitor
{
    private readonly TextVisitor textVisitor = new();
    
    public string Visit(Node node)
    {
        return node.Descendants != null ? $"<em>{string.Join(string.Empty, node.Descendants.Select(sentence => textVisitor.Visit(sentence)))}</em>" : "";
    }
}