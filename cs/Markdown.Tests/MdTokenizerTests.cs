using FluentAssertions;
using Markdown.Tokenizer;
using Markdown.Tokenizer.Tokens;
using NUnit.Framework;

namespace Markdown.Tests;
[TestFixture]
public class MdTokenizerTests
{
    private static IEnumerable<TestCaseData> MdTokenizeCases()
    {
        yield return new TestCaseData(null, new List<Token>
            {
                new Token(TypeOfToken.StartOfFile), 
                new Token(TypeOfToken.EndOfFile)
            })
            .SetName("StringIsNull");
        
        yield return new TestCaseData("", new List<Token>
            {
                new Token(TypeOfToken.StartOfFile), 
                new Token(TypeOfToken.EndOfFile)
            })
            .SetName("EmptyString");
        
        yield return new TestCaseData("SimpleText", new List<Token>
            {
                new Token(TypeOfToken.StartOfFile), 
                new Token(TypeOfToken.Word, "SimpleText"), 
                new Token(TypeOfToken.EndOfFile)
            })
            .SetName("OnlyText");
        
        yield return new TestCaseData("52", new List<Token>
            {
                new Token(TypeOfToken.StartOfFile), 
                new Token(TypeOfToken.Number, "52"), 
                new Token(TypeOfToken.EndOfFile)
            })
            .SetName("OnlyNumber");
        
        yield return new TestCaseData(" ", new List<Token>
            {
                new Token(TypeOfToken.StartOfFile), 
                new Token(TypeOfToken.Whitespace, " "), 
                new Token(TypeOfToken.EndOfFile)
            })
            .SetName("OnlyWhitespace");
        
        yield return new TestCaseData("_", new List<Token>
            {
                new Token(TypeOfToken.StartOfFile), 
                new Token(TypeOfToken.Underscore, "_"), 
                new Token(TypeOfToken.EndOfFile)
            })
            .SetName("OnlyUnderscore");
        
        yield return new TestCaseData("#", new List<Token>
            {
                new Token(TypeOfToken.StartOfFile), 
                new Token(TypeOfToken.Hash, "#"), 
                new Token(TypeOfToken.EndOfFile)
            })
            .SetName("OnlyHash");
        
        yield return new TestCaseData("\n", new List<Token>
            {
                new Token(TypeOfToken.StartOfFile), 
                new Token(TypeOfToken.Newline, "\n"), 
                new Token(TypeOfToken.EndOfFile)
            })
            .SetName("OnlyNewline");
        
        yield return new TestCaseData("$%^&", new List<Token>
            {
                new Token(TypeOfToken.StartOfFile),
                new Token(TypeOfToken.Word, "$%^&"),
                new Token(TypeOfToken.EndOfFile)
            })
            .SetName("DifferentCharacters");
        
        yield return new TestCaseData("#World 52 _Hello_", new List<Token>
            {
                new Token(TypeOfToken.StartOfFile),
                new Token(TypeOfToken.Hash, "#"),
                new Token(TypeOfToken.Word, "World"),
                new Token(TypeOfToken.Whitespace, " "),
                new Token(TypeOfToken.Number, "52"),
                new Token(TypeOfToken.Whitespace, " "),
                new Token(TypeOfToken.Underscore, "_"),
                new Token(TypeOfToken.Word, "Hello"),
                new Token(TypeOfToken.Underscore, "_"),
                new Token(TypeOfToken.EndOfFile)
            })
            .SetName("MixedTokens");
        
        yield return new TestCaseData("Hello\n_World_", new List<Token>
            {
                new Token(TypeOfToken.StartOfFile),
                new Token(TypeOfToken.Word, "Hello"),
                new Token(TypeOfToken.Newline, "\n"),
                new Token(TypeOfToken.Underscore, "_"),
                new Token(TypeOfToken.Word, "World"),
                new Token(TypeOfToken.Underscore, "_"),
                new Token(TypeOfToken.EndOfFile)
            })
            .SetName("MultilineText");
        
        yield return new TestCaseData("Hello   World", new List<Token>
            {
                new Token(TypeOfToken.StartOfFile),
                new Token(TypeOfToken.Word, "Hello"),
                new Token(TypeOfToken.Whitespace, " "),
                new Token(TypeOfToken.Whitespace, " "),
                new Token(TypeOfToken.Whitespace, " "),
                new Token(TypeOfToken.Word, "World"),
                new Token(TypeOfToken.EndOfFile)
            })
            .SetName("MultipleWhitespaces");
        
        yield return new TestCaseData(@"\_Escaped\_", new List<Token>
            {
                new Token(TypeOfToken.StartOfFile),
                new Token(TypeOfToken.Word, "_", 2),
                new Token(TypeOfToken.Word, "Escaped"),
                new Token(TypeOfToken.Word, "_", 2),
                new Token(TypeOfToken.EndOfFile)
            })
            .SetName("EscapedEmphasis");
        
        yield return new TestCaseData(@"\__Strong\__", new List<Token>
            {
                new Token(TypeOfToken.StartOfFile),
                new Token(TypeOfToken.Word, "_", 2),
                new Token(TypeOfToken.Underscore, "_"),
                new Token(TypeOfToken.Word, "Strong"),
                new Token(TypeOfToken.Word, "_", 2),
                new Token(TypeOfToken.Underscore, "_"),
                new Token(TypeOfToken.EndOfFile)
            })
            .SetName("EscapedStrong");

        yield return new TestCaseData(@"\# Heading", new List<Token>
            {
                new Token(TypeOfToken.StartOfFile),
                new Token(TypeOfToken.Word, "#", 2),
                new Token(TypeOfToken.Whitespace, " "),
                new Token(TypeOfToken.Word, "Heading"),
                new Token(TypeOfToken.EndOfFile)
            })
            .SetName("EscapedHash");

        yield return new TestCaseData(@"Hello\ World", new List<Token>
            {
                new Token(TypeOfToken.StartOfFile),
                new Token(TypeOfToken.Word, @"Hello\"),
                new Token(TypeOfToken.Whitespace, " "),
                new Token(TypeOfToken.Word, "World"),
                new Token(TypeOfToken.EndOfFile)
            })
            .SetName("EscapedWhitespace");

        yield return new TestCaseData(@"\\\\", new List<Token>
            {
                new Token(TypeOfToken.StartOfFile),
                new Token(TypeOfToken.Word, "\\", 2),
                new Token(TypeOfToken.Word, "\\", 2),
                new Token(TypeOfToken.EndOfFile)
            })
            .SetName("DoubleEscape");
    }
    
    [TestCaseSource(nameof(MdTokenizeCases))]
    public void Tokenize_CorrectlySplitsTestIntoTokens_When(string markdown, List<Token> expectedTokens)
    {
        var mdTokenizer = new MdTokenizer();
        
        var tokens = mdTokenizer.Tokenize(markdown);

        tokens.Should().BeEquivalentTo(expectedTokens, option => option.AllowingInfiniteRecursion());
    }
    
    [Test]
    public void Tokenize_CorrectlySplitsLongText()
    {
        var mdTokenizer = new MdTokenizer();
        var longText = new string('a', 1000);
    
        var tokens = mdTokenizer.Tokenize(longText);
    
        tokens.Count().Should().Be(3);
        tokens[1].Value.Should().Be(longText);
    }
}