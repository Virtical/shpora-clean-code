using System.Linq;
using Markdown.TokenParser.Nodes;

namespace Markdown.Converter.Visitors;

public class ListItemVisitor : IVisitor
{
    private readonly SentenceVisitor sentenceVisitor = new();
    
    public string Visit(Node node)
    {
        return $"<li>{SentencesFor(node)}</li>";
    }
    
    private string SentencesFor(Node paragraphNode)
    {
        if (paragraphNode.Descendants != null)
            return string.Join(string.Empty,
                paragraphNode.Descendants.Select(sentence => sentenceVisitor.Visit(sentence)));
        
        return "";
    }
}