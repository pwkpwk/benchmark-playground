using CommunityToolkit.HighPerformance;

namespace AmbientBytes.Algorithms;

public class GroupValueParser : IGroupValueParser
{
    IDictionary<string, string> IGroupValueParser.Parse(string text)
    {
        Dictionary<string, string> result = new();
        
        // Parse the most primitive format: {name1}={value1}, {name2}={value2}, ...
        // comma-separated list of key-value pairs separated by '=', so neither commas nor equal signs
        // are allowed in names and values.
        // Whitespace around names and values is ignored.
        foreach (var token in text.Tokenize(','))
        {
            AddGroupValue(result, token.Trim());
        }

        return result;
    }

    private static void AddGroupValue(Dictionary<string, string> values, ReadOnlySpan<char> expression)
    {
        ReadOnlySpan<char> name = ReadOnlySpan<char>.Empty;
        int index = 0;
        
        foreach (var token in expression.Tokenize('='))
        {
            switch (index)
            {
                case 0:
                    name = token.Trim();
                    break;
                
                case 1:
                    if (!name.IsEmpty)
                    {
                        values.TryAdd(name.ToString(), token.Trim().ToString());
                    }
                    return;
            }

            ++index;
        }
    }
}