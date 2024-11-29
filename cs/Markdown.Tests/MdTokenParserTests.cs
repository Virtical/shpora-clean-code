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
    private static IEnumerable<TestCaseData> MdTokenParseCases()
    {
        var tokenizer = new MdTokenizer();
        yield return new TestCaseData("Test",
                new Node(TypeOfNode.Body , new List<Node>
                {
                    new Node(TypeOfNode.Paragraph, new List<Node>
                    {
                        new Node(TypeOfNode.Text, "Test", 3)
                    }, 3)
                }, 3))
            .SetName("OnlyWord");
        
        yield return new TestCaseData("123",
                new Node(TypeOfNode.Body, new List<Node>
                {
                    new Node(TypeOfNode.Paragraph, new List<Node>
                    {
                        new Node(TypeOfNode.Text, "123", 3)
                    }, 3)
                }, 3))
            .SetName("OnlyNumber");
        
        yield return new TestCaseData("_Test_",
                new Node(TypeOfNode.Body, new List<Node>
                {
                    new Node(TypeOfNode.Paragraph, new List<Node>
                    {
                        new Node(TypeOfNode.Emphasis, new List<Node>
                        {
                            new Node(TypeOfNode.Text, "Test", 1)
                        }, 5)
                    }, 5)
                }, 5))
            .SetName("EmphasisWord");
        
        yield return new TestCaseData("__Test__",
                new Node(TypeOfNode.Body, new List<Node>
                {
                    new Node(TypeOfNode.Paragraph, new List<Node>
                    {
                        new Node(TypeOfNode.Strong, 
                            new List<Node>
                            {
                                new Node(TypeOfNode.Text, "Test", 1)
                            }, 7)
                    }, 7)
                }, 7))
            .SetName("StrongWord");
        
        yield return new TestCaseData("# Test",
                new Node(TypeOfNode.Body, new List<Node>
                {
                    new Node(TypeOfNode.Heading, new List<Node>
                    {
                        new Node(TypeOfNode.Text, "Test", 3)
                    }, 5)
                }, 5))
            .SetName("HashWord");
        
        yield return new TestCaseData("_785_",
                new Node(TypeOfNode.Body, new List<Node>
                {
                    new Node(TypeOfNode.Paragraph, new List<Node>
                    {
                        new Node(TypeOfNode.Emphasis, 
                            new List<Node>
                            {
                                new Node(TypeOfNode.Text, "785", 1)
                            }, 5)
                    }, 5)
                }, 5))
            .SetName("EmphasisNumber");
        
        yield return new TestCaseData("__543__",
                new Node(TypeOfNode.Body, new List<Node>
                {
                    new Node(TypeOfNode.Paragraph, new List<Node>
                    {
                        new Node(TypeOfNode.Strong, 
                            new List<Node>
                            {
                                new Node(TypeOfNode.Text, "543", 1)
                            }, 7)
                    }, 7)
                }, 7))
            .SetName("StrongNumber");
        
        yield return new TestCaseData("Test Test",
                new Node(TypeOfNode.Body, new List<Node>
                {
                    new Node(TypeOfNode.Paragraph, new List<Node>
                    {
                        new Node(TypeOfNode.Text, "Test", 2),
                        new Node(TypeOfNode.Text, " ", 1),
                        new Node(TypeOfNode.Text, "Test", 2)
                    }, 5)
                }, 5))
            .SetName("TextWithSeveralWords");
        
        yield return new TestCaseData("Test\nTest",
                new Node(TypeOfNode.Body, new List<Node>
                {
                    new Node(TypeOfNode.Paragraph, new List<Node>
                    {
                        new Node(TypeOfNode.Text, "Test", 2),
                        new Node(TypeOfNode.Text, "\n", 1),
                        new Node(TypeOfNode.Text, "Test", 2)
                    }, 5),
                }, 5))
            .SetName("TextWithSeveralLines");
        
        yield return new TestCaseData("Test\n\nTest",
                new Node(TypeOfNode.Body, new List<Node>
                {
                    new Node(TypeOfNode.Paragraph, new List<Node>
                    {
                        new Node(TypeOfNode.Text, "Test", 3)
                    }, 4),
                    new Node(TypeOfNode.Paragraph, new List<Node>
                    {
                        new Node(TypeOfNode.Text, "Test", 3)
                    }, 3)
                }, 7))
            .SetName("TextWithSeveralParagraphs");
        
        yield return new TestCaseData("# Test\n\nTest",
                new Node(TypeOfNode.Body, new List<Node>
                {
                    new Node(TypeOfNode.Heading, new List<Node>
                    {
                        new Node(TypeOfNode.Text, "Test", 3)
                    }, 6),
                    new Node(TypeOfNode.Paragraph, new List<Node>
                    {
                        new Node(TypeOfNode.Text, "Test", 3)
                    }, 3)
                }, 9))
            .SetName("HeadingAndParagraph");
        
        yield return new TestCaseData("__Strong _Emphasis_ Strong__",
                new Node(TypeOfNode.Body, new List<Node>
                {
                    new Node(TypeOfNode.Paragraph, new List<Node>
                    {
                        new Node(TypeOfNode.Strong, 
                            new List<Node>
                            {
                                new Node(TypeOfNode.Text, "Strong", 1),
                                new Node(TypeOfNode.Text, " ", 1), 
                                new Node(TypeOfNode.Emphasis, new List<Node>
                                {
                                    new Node(TypeOfNode.Text, "Emphasis", 1), 
                                }, 4),
                                new Node(TypeOfNode.Text, " ", 1), 
                                new Node(TypeOfNode.Text, "Strong", 1)
                            }, 13)
                    }, 13)
                }, 13))
            .SetName("EmphasisInsideStrong");
        
        yield return new TestCaseData("_Emphasis __Strong__ Emphasis_",
                new Node(TypeOfNode.Body, new List<Node>
                {
                    new Node(TypeOfNode.Paragraph, new List<Node>
                    {
                        new Node(TypeOfNode.Emphasis, new List<Node>
                        {
                            new Node(TypeOfNode.Text, "Emphasis", 1),
                            new Node(TypeOfNode.Text, " ", 1),
                            new Node(TypeOfNode.Text, "_", 1),
                            new Node(TypeOfNode.Text, "_", 1),
                            new Node(TypeOfNode.Text, "Strong", 1),
                            new Node(TypeOfNode.Text, "_", 1),
                            new Node(TypeOfNode.Text, "_", 1),
                            new Node(TypeOfNode.Text, " ", 1),
                            new Node(TypeOfNode.Text, "Emphasis", 1),
                        }, 13)
                    }, 13)
                }, 13))
            .SetName("StrongInsideEmphasis");
        
        yield return new TestCaseData("__Test",
                new Node(TypeOfNode.Body, new List<Node>
                {
                    new Node(TypeOfNode.Paragraph, new List<Node>
                    {
                        new Node(TypeOfNode.Text, "_", 2), 
                        new Node(TypeOfNode.Text, "_", 1), 
                        new Node(TypeOfNode.Text, "Test", 2)
                    }, 5)
                }, 5))
            .SetName("UnpairedStrong");
        
        yield return new TestCaseData("_Test",
                new Node(TypeOfNode.Body, new List<Node>
                {
                    new Node(TypeOfNode.Paragraph, new List<Node>
                    {
                        new Node(TypeOfNode.Text, "_", 2), 
                        new Node(TypeOfNode.Text, "Test", 2)
                    }, 4)
                }, 4))
            .SetName("UnpairedEmphasis");
        
        yield return new TestCaseData("Te_st Te_st",
                new Node(TypeOfNode.Body, new List<Node>
                {
                    new Node(TypeOfNode.Paragraph, new List<Node>
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
        
        yield return new TestCaseData("_ Test_",
                new Node(TypeOfNode.Body, new List<Node>
                {
                    new Node(TypeOfNode.Paragraph, new List<Node>
                    {
                        new Node(TypeOfNode.Text, "_", 2),
                        new Node(TypeOfNode.Text, " ", 1),
                        new Node(TypeOfNode.Text, "Test", 1),
                        new Node(TypeOfNode.Text, "_", 2)
                    }, 6)
                }, 6))
            .SetName("OpeningSingleUnderscoreFollowedByWhitespace");
        
        yield return new TestCaseData("__ Test__",
                new Node(TypeOfNode.Body, new List<Node>
                {
                    new Node(TypeOfNode.Paragraph, new List<Node>
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
        
        yield return new TestCaseData("_Test _",
                new Node(TypeOfNode.Body, new List<Node>
                {
                    new Node(TypeOfNode.Paragraph, new List<Node>
                    {
                        new Node(TypeOfNode.Text, "_", 2),
                        new Node(TypeOfNode.Text, "Test", 1),
                        new Node(TypeOfNode.Text, " ", 1),
                        new Node(TypeOfNode.Text, "_", 2)
                    }, 6)
                }, 6))
            .SetName("ClosingSingleUnderscorePrecededByWhitespace");
        
        yield return new TestCaseData("__Test __",
                new Node(TypeOfNode.Body, new List<Node>
                {
                    new Node(TypeOfNode.Paragraph, new List<Node>
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
        
        yield return new TestCaseData("Test_25_87_Test",
                new Node(TypeOfNode.Body, new List<Node>
                {
                    new Node(TypeOfNode.Paragraph, new List<Node>
                    {
                        new Node(TypeOfNode.Text, "Test", 2),
                        new Node(TypeOfNode.Text, "_", 1),
                        new Node(TypeOfNode.Text, "25", 1),
                        new Node(TypeOfNode.Text, "_", 1),
                        new Node(TypeOfNode.Text, "87", 1),
                        new Node(TypeOfNode.Text, "_", 1),
                        new Node(TypeOfNode.Text, "Test", 2)
                    }, 9)
                }, 9))
            .SetName("UnderscoreInsideTheTextWithNumbers");
        
        yield return new TestCaseData("_Te_st",
                new Node(TypeOfNode.Body, new List<Node>
                {
                    new Node(TypeOfNode.Paragraph, new List<Node>
                    {
                        new Node(TypeOfNode.Emphasis, new List<Node>
                        {
                            new Node(TypeOfNode.Text, "Te", 1),
                        }, 4),
                        new Node(TypeOfNode.Text, "st", 2)
                    }, 6)
                }, 6))
            .SetName("EmphasisPartOfWord");
        
        yield return new TestCaseData("_Test_ __Test__",
                new Node(TypeOfNode.Body, new List<Node>
                {
                    new Node(TypeOfNode.Paragraph, new List<Node>
                    {
                        new Node(TypeOfNode.Emphasis, new List<Node>
                        {
                            new Node(TypeOfNode.Text, "Test", 1),
                        }, 4),
                        new Node(TypeOfNode.Text, " ", 1), 
                        new Node(TypeOfNode.Strong, new List<Node>
                        {
                            new Node(TypeOfNode.Text, "Test", 1),
                        }, 7),
                    }, 11)
                }, 11))
            .SetName("WhitespaceSeparatedSelections");
        
        yield return new TestCaseData("# __Strong _Emphasis_ Strong__",
                new Node(TypeOfNode.Body, new List<Node>
                {
                    new Node(TypeOfNode.Heading, new List<Node>
                    {
                        new Node(TypeOfNode.Strong, 
                            new List<Node>
                            {
                                new Node(TypeOfNode.Text, "Strong", 1),
                                new Node(TypeOfNode.Text, " ", 1), 
                                new Node(TypeOfNode.Emphasis, new List<Node>
                                {
                                    new Node(TypeOfNode.Text, "Emphasis", 1), 
                                }, 4),
                                new Node(TypeOfNode.Text, " ", 1), 
                                new Node(TypeOfNode.Text, "Strong", 1)
                            }, 13)
                    }, 15)
                }, 15))
            .SetName("HeadingWithSelections");
    }
    
    [TestCaseSource(nameof(MdTokenParseCases))]
    public void Parse_BuildCorrectAST_When(string markdown, Node expectedSyntaxTree)
    {
        var mdTokenizer = new MdTokenizer();
        var mdTokenParser = new MdTokenParser();

        var tokens = mdTokenizer.Tokenize(markdown);
        var syntaxTree = mdTokenParser.Parse(tokens);

        syntaxTree.Should().BeEquivalentTo(expectedSyntaxTree, option => option.AllowingInfiniteRecursion());
    }
}