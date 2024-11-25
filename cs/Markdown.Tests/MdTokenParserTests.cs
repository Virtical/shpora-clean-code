using FluentAssertions;
using Markdown.Tokenizer;
using Markdown.Tokenizer.Tokens;
using Markdown.TokenParser;
using Markdown.TokenParser.Nodes;
using NUnit.Framework;

namespace Markdown.Tests;

[TestFixture]
public class MdTokenParserTests
{
    private static IEnumerable<TestCaseData> ParseCases()
    {
        var tokenizer = new MdTokenizer();
        yield return new TestCaseData(tokenizer.Tokenize("Тест"),
                new BodyNode(new List<INode>
                {
                    new ParagraphNode(new List<INode>
                    {
                        new Node(TypeOfNode.Text, "Тест", 3)
                    }, 3)
                }, 3))
            .SetName("OnlyWord");
        
        yield return new TestCaseData(tokenizer.Tokenize("_Тест_"),
                new BodyNode(new List<INode>
                {
                    new ParagraphNode(new List<INode>
                    {
                        new EmphasisNode(TypeOfNode.Emphasis, new List<INode>
                        {
                            new Node(TypeOfNode.Text, "Тест", 1)
                        }, 5)
                    }, 5)
                }, 5))
            .SetName("OnlyEmphasis");
        
        yield return new TestCaseData(tokenizer.Tokenize("__Тест__"),
                new BodyNode(new List<INode>
                {
                    new ParagraphNode(new List<INode>
                    {
                        new StrongNode(TypeOfNode.Strong, 
                            new List<INode>
                            {
                                new Node(TypeOfNode.Text, "Тест", 1)
                            }, 7)
                    }, 7)
                }, 7))
            .SetName("OnlyStrong");
        
        yield return new TestCaseData(tokenizer.Tokenize("# Тест"),
                new BodyNode(new List<INode>
                {
                    new HeadingNode(new List<INode>
                    {
                        new Node(TypeOfNode.Text, "Тест", 2)
                    }, 5)
                }, 5))
            .SetName("OnlyHash");
        
        yield return new TestCaseData(tokenizer.Tokenize("Тест Тест"),
                new BodyNode(new List<INode>
                {
                    new ParagraphNode(new List<INode>
                    {
                        new Node(TypeOfNode.Text, "Тест", 2),
                        new Node(TypeOfNode.Text, " ", 1),
                        new Node(TypeOfNode.Text, "Тест", 2)
                    }, 5)
                }, 5))
            .SetName("TextWithSeveralWords");
        
        yield return new TestCaseData(tokenizer.Tokenize("__Strong_Emphasis_Strong__"),
                new BodyNode(new List<INode>
                {
                    new ParagraphNode(new List<INode>
                    {
                        new StrongNode(TypeOfNode.Strong, 
                            new List<INode>
                            {
                                new Node(TypeOfNode.Text, "Strong", 1), 
                                new Node(TypeOfNode.Text, "_", 1), 
                                new Node(TypeOfNode.Text, "Emphasis", 1), 
                                new Node(TypeOfNode.Text, "_", 1), 
                                new Node(TypeOfNode.Text, "Strong", 1)
                            }, 11)
                    }, 11)
                }, 11))
            .SetName("EmphasisInsideStrong");
        
        yield return new TestCaseData(tokenizer.Tokenize("_Emphasis__Strong__Emphasis_"),
                new BodyNode(new List<INode>
                {
                    new ParagraphNode(new List<INode>
                    {
                        new EmphasisNode(TypeOfNode.Emphasis, new List<INode>
                        {
                            new Node(TypeOfNode.Text, "Emphasis", 1),
                            new Node(TypeOfNode.Text, "_", 1),
                            new Node(TypeOfNode.Text, "_", 1),
                            new Node(TypeOfNode.Text, "Strong", 1),
                            new Node(TypeOfNode.Text, "_", 1),
                            new Node(TypeOfNode.Text, "_", 1),
                            new Node(TypeOfNode.Text, "Emphasis", 1),
                        }, 11)
                    }, 11)
                }, 11))
            .SetName("StrongInsideEmphasis");
        
        yield return new TestCaseData(tokenizer.Tokenize("__Test"),
                new BodyNode(new List<INode>
                {
                    new ParagraphNode(new List<INode>
                    {
                        new Node(TypeOfNode.Text, "_", 2), 
                        new Node(TypeOfNode.Text, "_", 1), 
                        new Node(TypeOfNode.Text, "Test", 2)
                    }, 5)
                }, 5))
            .SetName("UnpairedStrong");
        
        yield return new TestCaseData(tokenizer.Tokenize("_Test"),
                new BodyNode(new List<INode>
                {
                    new ParagraphNode(new List<INode>
                    {
                        new Node(TypeOfNode.Text, "_", 2), 
                        new Node(TypeOfNode.Text, "Test", 2)
                    }, 4)
                }, 4))
            .SetName("UnpairedEmphasis");
        
        yield return new TestCaseData(tokenizer.Tokenize("Te_st Te_st"),
                new BodyNode(new List<INode>
                {
                    new ParagraphNode(new List<INode>
                    {
                        new Node(TypeOfNode.Text, "Te", 2),
                        new Node(TypeOfNode.Text, "_", 1),
                        new Node(TypeOfNode.Text, "st", 1),
                        new Node(TypeOfNode.Text, " ", 1),
                        new Node(TypeOfNode.Text, "Te", 1),
                        new Node(TypeOfNode.Text, "_", 1),
                        new Node(TypeOfNode.Text, "st", 2)
                    }, 9)
                }, 9))
            .SetName("EmphasisInDifferentWords");
        
        yield return new TestCaseData(tokenizer.Tokenize("_ Test_"),
                new BodyNode(new List<INode>
                {
                    new ParagraphNode(new List<INode>
                    {
                        new Node(TypeOfNode.Text, "_", 2),
                        new Node(TypeOfNode.Text, " ", 1),
                        new Node(TypeOfNode.Text, "Test", 1),
                        new Node(TypeOfNode.Text, "_", 2)
                    }, 6)
                }, 6))
            .SetName("OpeningSingleUnderscoreFollowedByWhitespace");
        
        yield return new TestCaseData(tokenizer.Tokenize("__ Test__"),
                new BodyNode(new List<INode>
                {
                    new ParagraphNode(new List<INode>
                    {
                        new Node(TypeOfNode.Text, "_", 2),
                        new Node(TypeOfNode.Text, "_", 1),
                        new Node(TypeOfNode.Text, " ", 1),
                        new Node(TypeOfNode.Text, "Test", 1),
                        new Node(TypeOfNode.Text, "_", 1),
                        new Node(TypeOfNode.Text, "_", 2)
                    }, 8)
                }, 8))
            .SetName("OpeningDoubleUnderscoreFollowedByWhitespace");
        
        yield return new TestCaseData(tokenizer.Tokenize("_Test _"),
                new BodyNode(new List<INode>
                {
                    new ParagraphNode(new List<INode>
                    {
                        new Node(TypeOfNode.Text, "_", 2),
                        new Node(TypeOfNode.Text, "Test", 1),
                        new Node(TypeOfNode.Text, " ", 1),
                        new Node(TypeOfNode.Text, "_", 2)
                    }, 6)
                }, 6))
            .SetName("ClosingSingleUnderscorePrecededByWhitespace");
        
        yield return new TestCaseData(tokenizer.Tokenize("__Test __"),
                new BodyNode(new List<INode>
                {
                    new ParagraphNode(new List<INode>
                    {
                        new Node(TypeOfNode.Text, "_", 2),
                        new Node(TypeOfNode.Text, "_", 1),
                        new Node(TypeOfNode.Text, "Test", 1),
                        new Node(TypeOfNode.Text, " ", 1),
                        new Node(TypeOfNode.Text, "_", 1),
                        new Node(TypeOfNode.Text, "_", 2)
                    }, 8)
                }, 8))
            .SetName("ClosingDoubleUnderscorePrecededByWhitespace");
    }
    
    [TestCaseSource(nameof(ParseCases))]
    public void Parse_BuildCorrectAST_When(TokenList tokens, BodyNode expectedSyntaxTree)
    {
        var mdTokenizer = new MdTokenParser();
        var syntaxTree = mdTokenizer.Parse(tokens);

        syntaxTree.Should().BeEquivalentTo(expectedSyntaxTree, option => option.AllowingInfiniteRecursion());
    }
}