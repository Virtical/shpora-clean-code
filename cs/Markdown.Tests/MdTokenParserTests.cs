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
        yield return new TestCaseData(tokenizer.Tokenize("Test"),
                new BodyNode(new List<INode>
                {
                    new ParagraphNode(new List<INode>
                    {
                        new Node(TypeOfNode.Text, "Test", 3)
                    }, 3)
                }, 3))
            .SetName("OnlyWord");
        
        yield return new TestCaseData(tokenizer.Tokenize("123"),
                new BodyNode(new List<INode>
                {
                    new ParagraphNode(new List<INode>
                    {
                        new Node(TypeOfNode.Text, "123", 3)
                    }, 3)
                }, 3))
            .SetName("OnlyNumber");
        
        yield return new TestCaseData(tokenizer.Tokenize("_Test_"),
                new BodyNode(new List<INode>
                {
                    new ParagraphNode(new List<INode>
                    {
                        new EmphasisNode(TypeOfNode.Emphasis, new List<INode>
                        {
                            new Node(TypeOfNode.Text, "Test", 1)
                        }, 5)
                    }, 5)
                }, 5))
            .SetName("EmphasisWord");
        
        yield return new TestCaseData(tokenizer.Tokenize("__Test__"),
                new BodyNode(new List<INode>
                {
                    new ParagraphNode(new List<INode>
                    {
                        new StrongNode(TypeOfNode.Strong, 
                            new List<INode>
                            {
                                new Node(TypeOfNode.Text, "Test", 1)
                            }, 7)
                    }, 7)
                }, 7))
            .SetName("StrongWord");
        
        yield return new TestCaseData(tokenizer.Tokenize("# Test"),
                new BodyNode(new List<INode>
                {
                    new HeadingNode(new List<INode>
                    {
                        new Node(TypeOfNode.Text, "Test", 3)
                    }, 5)
                }, 5))
            .SetName("HashWord");
        
        yield return new TestCaseData(tokenizer.Tokenize("_785_"),
                new BodyNode(new List<INode>
                {
                    new ParagraphNode(new List<INode>
                    {
                        new EmphasisNode(TypeOfNode.Emphasis, 
                            new List<INode>
                            {
                                new Node(TypeOfNode.Text, "785", 1)
                            }, 5)
                    }, 5)
                }, 5))
            .SetName("EmphasisNumber");
        
        yield return new TestCaseData(tokenizer.Tokenize("__543__"),
                new BodyNode(new List<INode>
                {
                    new ParagraphNode(new List<INode>
                    {
                        new StrongNode(TypeOfNode.Strong, 
                            new List<INode>
                            {
                                new Node(TypeOfNode.Text, "543", 1)
                            }, 7)
                    }, 7)
                }, 7))
            .SetName("StrongNumber");
        
        yield return new TestCaseData(tokenizer.Tokenize("Test Test"),
                new BodyNode(new List<INode>
                {
                    new ParagraphNode(new List<INode>
                    {
                        new Node(TypeOfNode.Text, "Test", 2),
                        new Node(TypeOfNode.Text, " ", 1),
                        new Node(TypeOfNode.Text, "Test", 2)
                    }, 5)
                }, 5))
            .SetName("TextWithSeveralWords");
        
        yield return new TestCaseData(tokenizer.Tokenize("__Strong _Emphasis_ Strong__"),
                new BodyNode(new List<INode>
                {
                    new ParagraphNode(new List<INode>
                    {
                        new StrongNode(TypeOfNode.Strong, 
                            new List<INode>
                            {
                                new Node(TypeOfNode.Text, "Strong", 1), 
                                new EmphasisNode(TypeOfNode.Emphasis, new List<INode>
                                {
                                    new Node(TypeOfNode.Text, "Emphasis", 1), 
                                }, 5),
                                new Node(TypeOfNode.Text, "Strong", 1)
                            }, 13)
                    }, 13)
                }, 13))
            .SetName("EmphasisInsideStrong");
        
        yield return new TestCaseData(tokenizer.Tokenize("_Emphasis __Strong__ Emphasis_"),
                new BodyNode(new List<INode>
                {
                    new ParagraphNode(new List<INode>
                    {
                        new EmphasisNode(TypeOfNode.Emphasis, new List<INode>
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
        
        yield return new TestCaseData(tokenizer.Tokenize("Test_25_87_Test"),
                new BodyNode(new List<INode>
                {
                    new ParagraphNode(new List<INode>
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
        
        yield return new TestCaseData(tokenizer.Tokenize("_Te_st"),
                new BodyNode(new List<INode>
                {
                    new ParagraphNode(new List<INode>
                    {
                        new EmphasisNode(TypeOfNode.Emphasis, new List<INode>
                        {
                            new Node(TypeOfNode.Text, "Te", 1),
                        }, 4),
                        new Node(TypeOfNode.Text, "st", 2)
                    }, 6)
                }, 6))
            .SetName("EmphasisPartOfWord");
        
        yield return new TestCaseData(tokenizer.Tokenize("_Test_ __Test__"),
                new BodyNode(new List<INode>
                {
                    new ParagraphNode(new List<INode>
                    {
                        new EmphasisNode(TypeOfNode.Emphasis, new List<INode>
                        {
                            new Node(TypeOfNode.Text, "Test", 1),
                        }, 4),
                        new StrongNode(TypeOfNode.Strong, new List<INode>
                        {
                            new Node(TypeOfNode.Text, "Test", 1),
                        }, 7),
                    }, 11)
                }, 11))
            .SetName("WhitespaceSeparatedSelections");
        
        yield return new TestCaseData(tokenizer.Tokenize("# __Strong _Emphasis_ Strong__"),
                new BodyNode(new List<INode>
                {
                    new HeadingNode(new List<INode>
                    {
                        new StrongNode(TypeOfNode.Strong, 
                            new List<INode>
                            {
                                new Node(TypeOfNode.Text, "Strong", 1), 
                                new EmphasisNode(TypeOfNode.Emphasis, new List<INode>
                                {
                                    new Node(TypeOfNode.Text, "Emphasis", 1), 
                                }, 5),
                                new Node(TypeOfNode.Text, "Strong", 1)
                            }, 13)
                    }, 15)
                }, 15))
            .SetName("HeadingWithSelections");
    }
    
    [TestCaseSource(nameof(MdTokenParseCases))]
    public void Parse_BuildCorrectAST_When(TokenList tokens, BodyNode expectedSyntaxTree)
    {
        var mdTokenizer = new MdTokenParser();
        var syntaxTree = mdTokenizer.Parse(tokens);

        syntaxTree.Should().BeEquivalentTo(expectedSyntaxTree, option => option.AllowingInfiniteRecursion());
    }
}