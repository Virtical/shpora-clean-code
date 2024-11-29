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
    [TestCase("Test", "<p>Test</p>", TestName = "ParagraphWord")]
    [TestCase("# Test", "<h1>Test</h1>", TestName = "HeadingWord")]
    [TestCase("_Test_", "<p><em>Test</em></p>", TestName = "ParagraphEmphasisWord")]
    [TestCase("__Test__", "<p><strong>Test</strong></p>", TestName = "ParagraphStrongWord")]
    [TestCase("# __Test__", "<h1><strong>Test</strong></h1>", TestName = "StrongWord")]
    [TestCase("_Test_ __Test__", "<p><em>Test</em> <strong>Test</strong></p>", TestName = "ParagraphEmphasisAndStrong")]
    [TestCase("# _Test_ __Test__", "<h1><em>Test</em> <strong>Test</strong></h1>", TestName = "HeadingEmphasisAndStrong")]
    [TestCase("__Strong _Emphasis_ Strong__", "<p><strong>Strong <em>Emphasis</em> Strong</strong></p>", TestName = "ParagraphEmphasisInsideStrong")]
    [TestCase("# __Strong _Emphasis_ Strong__", "<h1><strong>Strong <em>Emphasis</em> Strong</strong></h1>", TestName = "HeadingEmphasisInsideStrong")]
    [TestCase("Hello\nWorld", "<p>Hello\nWorld</p>", TestName = "TextWithSeveralLines")]
    [TestCase("Test\n\nTest", "<p>Test</p>\n\n<p>Test</p>", TestName = "TextWithSeveralParagraphs")]
    [TestCase("# Test\n\nTest", "<h1>Test</h1>\n\n<p>Test</p>", TestName = "HeadingAndParagraph")]
    [TestCase("- Пункт первый", "<p><ul><li>Пункт первый</li></ul></p>", TestName = "UnorderedListWithOneListItem")]
    [TestCase("- Пункт первый\n- Пункт второй\n- Пункт третий", "<p><ul><li>Пункт первый</li><li>Пункт второй</li><li>Пункт третий</li></ul></p>", TestName = "UnorderedListWithSeveralListItem")]
    [TestCase("- Уровень 1.1\n    - Уровень 2.1\n- Уровень 1.2\n    - Уровень 2.1\n    - Уровень 2.2", "<p><ul><li>Уровень 1.1<ul><li>Уровень 2.1</li></ul></li><li>Уровень 1.2<ul><li>Уровень 2.1</li><li>Уровень 2.2</li></ul></li></ul></p>", TestName = "NestedUnorderedLists")]
    
    public void Parse_BuildCorrectAST_When(string markdown, string expectedHtml)
    {
        var tokenizer = new MdTokenizer();
        var tokenParser = new MdTokenParser();
        var mdToHtmlConverter = new MdToHtmlConverter();

        var tokens = tokenizer.Tokenize(markdown);
        var ast = tokenParser.Parse(tokens);
        var html = mdToHtmlConverter.RenderTokens(ast);

        html.Should().Be(expectedHtml);
    }
}