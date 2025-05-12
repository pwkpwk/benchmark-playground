namespace AmbientBytes.Algorithms;

public interface IGroupValueParser
{
    IDictionary<string, string> Parse(string text);
}