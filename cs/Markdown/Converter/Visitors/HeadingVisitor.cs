using System.Linq;
using Markdown.TokenParser.Nodes;

namespace Markdown.Converter.Visitors;

public class HeadingVisitor : IVisitor
{
    private readonly SentenceVisitor sentenceVisitor = new();
    
    public string Visit(Node paragraphNode)
    {
        return $"<h1>{SentencesFor(paragraphNode)}</h1>";
    }

    private string SentencesFor(Node paragraphNode)
    {
        return string.Join(string.Empty, paragraphNode.Descendants.Select(sentence => sentenceVisitor.Visit(sentence)));
    }
}