using CommunityToolkit.HighPerformance;

namespace AmbientBytes.Algorithms;

public static class Tokens
{
    public static bool IsTokenPresent(string token, string csv)
    {
        var tokenSpan = token.AsSpan().Trim();

        if (!tokenSpan.IsEmpty)
        {
            foreach (var span in csv.Tokenize(','))
            {
                if (span.Trim().Equals(tokenSpan, StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }
            }
        }

        return false;
    }
}