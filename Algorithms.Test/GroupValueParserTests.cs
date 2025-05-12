namespace AmbientBytes.Algorithms.Test;

[TestFixture]
public class GroupValueParserTests
{
    private IGroupValueParser _parser;

    [SetUp]
    public void SetUpTest()
    {
        _parser = new GroupValueParser();
    }
    
    [Test]
    public void ValidInput_Parse_CorrectDictionary()
    {
        var values = _parser.Parse(" a=--A--, b = --B--, cc = --CC--  ");
        
        Assert.Multiple(() =>
        {
            Assert.That(values.Count, Is.EqualTo(3));
            Assert.That(values["a"], Is.EqualTo("--A--"));
            Assert.That(values["b"], Is.EqualTo("--B--"));
            Assert.That(values["cc"], Is.EqualTo("--CC--"));
        });
    }
    
    [Test]
    public void DuplicateValue_Parse_FirstOccurenceIsUsed()
    {
        var values = _parser.Parse(" a=--A--, b = --B--, cc = --CC-- , b = --BB-- ");
        
        Assert.Multiple(() =>
        {
            Assert.That(values.Count, Is.EqualTo(3));
            Assert.That(values["a"], Is.EqualTo("--A--"));
            Assert.That(values["b"], Is.EqualTo("--B--"));
            Assert.That(values["cc"], Is.EqualTo("--CC--"));
        });
    }
    
    [TestCase(" a=--A--,b  , cc = --CC--  ")]
    [TestCase(" a=--A--, b, cc = --CC--  ")]
    [TestCase(" a=--A--,b , cc = --CC--  ")]
    [TestCase(" a=--A--,   , cc = --CC--  ")]
    [TestCase(" a=--A--, ==  , cc = --CC--  ")]
    public void IncorrectValue_Parse_SkipsIncorrectValue(string input)
    {
        var values = _parser.Parse(input);
        
        Assert.Multiple(() =>
        {
            Assert.That(values.Count, Is.EqualTo(2));
            Assert.That(values["a"], Is.EqualTo("--A--"));
            Assert.That(values["cc"], Is.EqualTo("--CC--"));
        });
    }
}