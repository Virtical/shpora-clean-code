using System;
using System.Collections.Generic;
using Markdown.Tokenizer.Tokens;
using Markdown.Tokenizer.TokenScanners;

namespace Markdown.Tokenizer;
public class MdTokenizer : IMdTokenizer
{
    private static readonly List<ITokenScanner> TokenScanners = new List<ITokenScanner>
    {
        new SimpleScanner(),
        new NumberScanner(),
        new TextScanner()
    };
    public TokenList Tokenize(string text)
    {
        var tokensAsLinkedList = TokensAsLinkedList(text);
        tokensAsLinkedList.AddFirst(new Token(TypeOfToken.StartOfFile));
        tokensAsLinkedList.AddLast(new Token(TypeOfToken.EndOfFile));
        
        return new TokenList(tokensAsLinkedList);
    }

    private static LinkedList<Token> TokensAsLinkedList(string text)
    {
        var tokens = new LinkedList<Token>();
        
        if (string.IsNullOrEmpty(text))
        {
            return tokens;
        }

        var token = ScanOneToken(text);
        var remainingTokens = TokensAsLinkedList(text[token.Length..]);
        remainingTokens.AddFirst(token);
        
        return remainingTokens;
    }
    
    private static Token ScanOneToken(string plainMarkdown)
    {
        foreach (var scanner in TokenScanners)
        {
            var token = scanner.FromString(plainMarkdown);
            
            if (token != null)
            {
                return token;
            }
        }
        
        throw new Exception($"The scanners could not match the given input: {plainMarkdown}");
    }
}
