using System;
using System.Linq;
using Markdown.TokenParser.Nodes;

namespace Markdown.Converter.Visitors;

public class UnorderedListVisitor : IVisitor
{
    private readonly ListItemVisitor sentenceVisitor = new();
    
    public string Visit(Node paragraphNode)
    {
        return $"<ul>{SentencesFor(paragraphNode)}</ul>";
    }

    private string SentencesFor(Node paragraphNode)
    {
        if (paragraphNode.Descendants != null)
            return string.Join(string.Empty,
                paragraphNode.Descendants.Select(sentence => sentenceVisitor.Visit(sentence)));
        
        return "";
    }
}