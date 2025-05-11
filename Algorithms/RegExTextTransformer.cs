using System.Text;
using System.Text.RegularExpressions;

namespace AmbientBytes.Algorithms;

/// <summary>
/// Create a new text transformer that replaces groups matched in a string by the regular expression
/// with values from the supplied dictionary.
/// </summary>
/// <param name="regEx">Regular expression string with marked groups</param>
/// <param name="replacements">Dictionary of matched group replacements.</param>
public class RegExTextTransformer(
    string regEx,
    IDictionary<string, string> replacements) : ITextTransformer
{
    private readonly Regex _regex = new(regEx);
    
    string ITextTransformer.Transform(string text)
    {
        var match = _regex.Match(text);
        return match.Success ? Transform(text, match) : text;
    }

    private string Transform(string text, Match match)
    {
        var builder = new StringBuilder(text.Length);
        int offset = 0;
        bool firstMatch = true;
        var textSpan = text.AsSpan();

        foreach (Group group in match.Groups)
        {
            if (firstMatch)
            {
                firstMatch = false;
            }
            else
            {
                var gap = textSpan.Slice(offset, group.Index - offset);
                offset = group.Index + group.Length;
                builder.Append(gap);

                if (replacements.TryGetValue(group.Name, out var replacement))
                {
                    builder.Append(replacement);
                }
            }
        }

        if (offset < textSpan.Length)
        {
            builder.Append(textSpan.Slice(offset));
        }
        
        return builder.ToString();
    }
}