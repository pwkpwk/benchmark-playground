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
    IDictionary<string, string> replacements,
    RegexOptions options = RegexOptions.Compiled | RegexOptions.IgnoreCase)
    : ITextTransformer
{
    private readonly Regex _regex = new(regEx, options);
    
    string ITextTransformer.Transform(string text)
    {
        var match = _regex.Match(text);
        return match.Success ? Transform(text, match) : text;
    }

    private static ReadOnlySpan<Group> SortGroups(GroupCollection groups)
    {
        var rl = (IReadOnlyList<Group>)groups;
        Group[] array = rl.ToArray();

        var span = array.AsSpan().Slice(1);
        
        span.Sort(GroupComparer.Comparer);
        return span;
    }

    private string Transform(string text, Match match)
    {
        var builder = new StringBuilder(text.Length);
        int offset = 0;
        var textSpan = text.AsSpan();

        foreach (Group group in SortGroups(match.Groups))
        {
            if (group.Index > offset)
            {
                var gap = textSpan.Slice(offset, group.Index - offset);
                builder.Append(gap);
            }
            offset = group.Index + group.Length;

            if (replacements.TryGetValue(group.Name, out var replacement))
            {
                builder.Append(replacement);
            }
        }

        if (offset < textSpan.Length)
        {
            builder.Append(textSpan.Slice(offset));
        }
        
        return builder.ToString();
    }
    
    private sealed class GroupComparer : IComparer<Group>
    {
        public static readonly IComparer<Group> Comparer = new GroupComparer();
        
        int IComparer<Group>.Compare(Group? x, Group? y) => x!.Index.CompareTo(y!.Index);
    }
}