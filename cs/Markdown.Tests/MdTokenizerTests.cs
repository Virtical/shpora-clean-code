using FluentAssertions;
using Markdown.Tokenizer;
using Markdown.Tokenizer.Tokens;
using NUnit.Framework;

namespace Markdown.Tests;
[TestFixture]
public class MdTokenizerTests
{
    private static IEnumerable<TestCaseData> TokenizeCases()
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
        
        yield return new TestCaseData("ОбычныйТекст", new List<Token>
            {
                new Token(TypeOfToken.StartOfFile), 
                new Token(TypeOfToken.Word, "ОбычныйТекст"), 
                new Token(TypeOfToken.EndOfFile)
            })
            .SetName("OnlyText");
        
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
        
        yield return new TestCaseData("#World _Hello_", new List<Token>
            {
                new Token(TypeOfToken.StartOfFile),
                new Token(TypeOfToken.Hash, "#"),
                new Token(TypeOfToken.Word, "World"),
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
    }
    
    [TestCaseSource(nameof(TokenizeCases))]
    public void Tokenize_CorrectlySplitsTestIntoTokens_When(string text, List<Token> expectedTokens)
    {
        var mdTokenizer = new MdTokenizer();
        var tokens = mdTokenizer.Tokenize(text);

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