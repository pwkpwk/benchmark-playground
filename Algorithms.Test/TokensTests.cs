namespace AmbientBytes.Algorithms.Test;

[TestFixture]
public class TokensTests
{
    [TestCase("Wort", "Wort", ExpectedResult = true)]
    [TestCase("Wort", "wort", ExpectedResult = true)]
    [TestCase("Wort", "WORT", ExpectedResult = true)]
    [TestCase("  Wort   ", "Wort", ExpectedResult = true)]
    [TestCase("Wort,Work,Worp", "Wort", ExpectedResult = true)]
    [TestCase("Wort,Work,Worp", "Work", ExpectedResult = true)]
    [TestCase("Wort,Work,Worp", " work ", ExpectedResult = true)]
    [TestCase("Wort,Work,Worp", "Worp", ExpectedResult = true)]
    [TestCase("Wort,Work,  Worp  ", "Worp", ExpectedResult = true)]
    [TestCase("Wort, Work, Worp  ", "Worp", ExpectedResult = true)]
    public bool TokenPresent_IsTokenPresent_ReturnsTrue(string csv, string token) => Tokens.IsTokenPresent(token, csv);

    [TestCase(" Wort, Work,  Worp  ", "Word", ExpectedResult = false)]
    [TestCase("", "Word", ExpectedResult = false)]
    [TestCase(" ", " ", ExpectedResult = false)]
    public bool TokenNotPresent_IsTokenPresent_ReturnsFalse(string csv, string token) => Tokens.IsTokenPresent(token, csv);
}