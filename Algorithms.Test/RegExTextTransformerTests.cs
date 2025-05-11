namespace AmbientBytes.Algorithms.Test;

[TestFixture]
public class RegExTextTransformerTests
{
    private readonly IDictionary<string, string> _replacements = new Dictionary<string, string>
    {
        { "a", "Letter-A" },
        { "b", "Letter-B" },
        { "c", "Letter-C" }
    };

    [TestCase("^(?<a>\\s*)\\-BAM\\!$",
        "       -BAM!",
        ExpectedResult = "Letter-A-BAM!")]
    [TestCase("^https://(?<a>bom)\\.example\\.com\\/(?<b>[Dd][Ii][Nn][Gg])\\/dong$",
        "https://bom.example.com/ding/dong",
        ExpectedResult = "https://Letter-A.example.com/Letter-B/dong")]
    [TestCase("^https://(?<a>bom)\\.example\\.com\\/(?<b>[Dd][Ii][Nn][Gg])\\/dong$",
        "https://bom.example.com/Ding/dong",
        ExpectedResult = "https://Letter-A.example.com/Letter-B/dong")]
    [TestCase("^https://(?<a>bom)\\.example\\.com\\/(?<b>[Dd][Ii][Nn][Gg])\\/dong$",
        "https://bom.example.com/ping/pong",
        ExpectedResult = "https://bom.example.com/ping/pong")]
    [TestCase("^(?<h>\\s*)https://www\\.example\\.com\\/path\\/\\?a=b(?<h>\\s*)$",
        "https://www.example.com/path/?a=b",
        ExpectedResult = "https://www.example.com/path/?a=b")]
    [TestCase("^(\\s*)https://www\\.example\\.com\\/path\\/\\?a=b(\\s*)$",
        "   https://www.example.com/path/?a=b  ",
        ExpectedResult = "https://www.example.com/path/?a=b")]
    public string Plop(string regEx, string text)
    {
        ITextTransformer transformer = new RegExTextTransformer(regEx, _replacements);
        return transformer.Transform(text);
    }
}