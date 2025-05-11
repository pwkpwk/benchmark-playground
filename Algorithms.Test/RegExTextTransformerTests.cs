namespace AmbientBytes.Algorithms.Test;

[TestFixture]
public class RegExTextTransformerTests
{
    private readonly IDictionary<string, string> _replacements = new Dictionary<string, string>
    {
        { "a", "--A--" },
        { "b", "-B-" },
        { "c", "-C-" }
    };

    [TestCase("^(?<a>\\s*)\\-BAM\\!$",
        "       -BAM!",
        ExpectedResult = "--A---BAM!")]
    [TestCase("^https://(?<a>bom)\\.example\\.com\\/(?<b>[Dd][Ii][Nn][Gg])\\/dong$",
        "https://bom.example.com/ding/dong",
        ExpectedResult = "https://--A--.example.com/-B-/dong")]
    [TestCase("^https://(?<a>bom)\\.example\\.com\\/(?<b>[Dd][Ii][Nn][Gg])\\/dong$",
        "https://bom.example.com/Ding/dong",
        ExpectedResult = "https://--A--.example.com/-B-/dong")]
    [TestCase("^https://(?<a>bom)\\.example\\.com\\/(?<b>[Dd][Ii][Nn][Gg])\\/dong$",
        "https://bom.example.com/ping/pong",
        ExpectedResult = "https://bom.example.com/ping/pong")]
    [TestCase("^(?<h>\\s*)https://www\\.example\\.com\\/path\\/\\?a=b(?<h>\\s*)$",
        "https://www.example.com/path/?a=b",
        ExpectedResult = "https://www.example.com/path/?a=b")]
    [TestCase("^(\\s*)https://www\\.example\\.com\\/path\\/\\?a=b(\\s*)$",
        "   https://www.example.com/path/?a=b  ",
        ExpectedResult = "https://www.example.com/path/?a=b")]
    [TestCase(@"^(\s*)https://(?<a>benchmark).example.com/path/(?<b>beta)[^\s]*(\s*)$",
        "   https://benchmark.example.com/path/beta?q=query   ",
        ExpectedResult = "https://--A--.example.com/path/-B-?q=query")]
    public string Transform_CorrectResult(string regEx, string text)
    {
        ITextTransformer transformer = new RegExTextTransformer(regEx, _replacements);
        return transformer.Transform(text);
    }
}