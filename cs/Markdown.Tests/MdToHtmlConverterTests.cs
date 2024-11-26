using FluentAssertions;
using Markdown.Converter;
using Markdown.Tokenizer;
using Markdown.Tokenizer.Tokens;
using Markdown.TokenParser;
using Markdown.TokenParser.Nodes;
using NUnit.Framework;

namespace Markdown.Tests;

[TestFixture]
public class MdToHtmlConverterTests
{
    private static IEnumerable<TestCaseData> MdToHtmlConverterCases()
    {
        var tokenizer = new MdTokenizer();
        var tokenParser = new MdTokenParser();
        
        yield return new TestCaseData(tokenParser.Parse(tokenizer.Tokenize("Test")), "<p>Test</p>").SetName("ParagraphWord");
        yield return new TestCaseData(tokenParser.Parse(tokenizer.Tokenize("# Test")), "<h1>Test</h1>").SetName("HeadingWord");
        yield return new TestCaseData(tokenParser.Parse(tokenizer.Tokenize("_Test_")), "<p><em>Test</em></p>").SetName("ParagraphEmphasisWord");
        yield return new TestCaseData(tokenParser.Parse(tokenizer.Tokenize("__Test__")), "<p><strong>Test</strong></p>").SetName("ParagraphStrongWord");
        yield return new TestCaseData(tokenParser.Parse(tokenizer.Tokenize("# __Test__")), "<h1><strong>Test</strong></h1>").SetName("StrongWord");
    }
    
    [TestCaseSource(nameof(MdToHtmlConverterCases))]
    public void Parse_BuildCorrectAST_When(BodyNode bodyNode, string expectedHtml)
    {
        var mdToHtmlConverter = new MdToHtmlConverter();
        var html = mdToHtmlConverter.RenderTokens(bodyNode);

        html.Should().Be(expectedHtml);
    }
}