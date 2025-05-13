using System.Text.RegularExpressions;
using AmbientBytes.Algorithms;
using BenchmarkDotNet.Attributes;

namespace AmbientBytes.Benchmark;

[MemoryDiagnoser]
public class RegExTextTransformerMatchBenchmarks
{
    private IDictionary<string, string> _variables;
    private Regex _raw;
    private Regex _rawCompiled;
    private ITextTransformer _interpreted;
    private ITextTransformer _compiled;

    private const string Expression = @"^(\s*)https://(?<a>benchmark).example.com/path/(?<b>beta)[^\s]*(\s*)$";
    private const string Input = "   https://benchmark.example.com/path/beta?q=query   ";
    private const string Replacement = "https://$(a).example.com/path/$(b)?q=query";

    [Params(1, 1000)] public int Repetitions;

    [GlobalSetup]
    public void SetUp()
    {
        _variables = new Dictionary<string, string> { { "a", "-A-" }, { "b", "-B-" }, { "c", "-C-" } };
        _raw = new Regex(Expression);
        _raw = new Regex(Expression, RegexOptions.Compiled);
        _interpreted = new RegExTextTransformer(Expression, _variables, RegexOptions.None);
        _compiled = new RegExTextTransformer(Expression, _variables, RegexOptions.Compiled);
    }

    [Benchmark(Baseline = true)]
    public string RegexReplace()
    {
        for (int i = 0; i < Repetitions; ++i)
        {
            _ = _raw.Replace(Input, Replacement);
        }

        return _raw.Replace(Input, Replacement);
    }

    [Benchmark]
    public string RegexCompiledReplace()
    {
        for (int i = 0; i < Repetitions; ++i)
        {
            _ = _rawCompiled.Replace(Input, Replacement);
        }

        return _rawCompiled.Replace(Input, Replacement);
    }

    [Benchmark]
    public string InterpretedExpression()
    {
        for (int i = 0; i < Repetitions; ++i)
        {
            _ = _interpreted.Transform(Input);
        }

        return _interpreted.Transform(Input);
    }

    [Benchmark]
    public string CompiledExpression()
    {
        for (int i = 0; i < Repetitions; ++i)
        {
            _ = _compiled.Transform(Input);
        }

        return _compiled.Transform(Input);
    }
}