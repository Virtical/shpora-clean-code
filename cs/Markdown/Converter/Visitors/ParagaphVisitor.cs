﻿using System.Linq;
using Markdown.TokenParser.Nodes;

namespace Markdown.Converter.Visitors;

public class ParagraphVisitor : IVisitor
{
    private readonly SentenceVisitor sentenceVisitor = new();
    
    public string Visit(INode paragraphNode)
    {
        return $"<p>{SentencesFor(paragraphNode)}</p>";
    }

    private string SentencesFor(INode paragraphNode)
    {
        return string.Join(string.Empty, paragraphNode.Descendants.Select(sentence => sentenceVisitor.Visit(sentence)));
    }
}