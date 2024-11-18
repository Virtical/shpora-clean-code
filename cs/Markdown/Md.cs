namespace Markdown;
public class Md
{
    private readonly MdParser parser = new MdParser();
    private readonly MdRenderer renderer = new MdRenderer();

    public Md(MdParser parser, MdRenderer renderer)
    {
        this.parser = parser;
        this.renderer = renderer;
    }

    public string Render(string text)
    {
        var tokens = parser.SplitIntoTokens(text);
        var renderedText = renderer.RenderTokens(tokens);

        return renderedText;
    }
}
