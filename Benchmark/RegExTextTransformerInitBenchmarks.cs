using System.Text.RegularExpressions;
using AmbientBytes.Algorithms;
using BenchmarkDotNet.Attributes;

namespace AmbientBytes.Benchmark;

[MemoryDiagnoser]
public class RegExTextTransformerInitBenchmarks
{
    private IDictionary<string, string> _variables;

    private const string Expression = @"^(\s*)https://(?<a>benchmark).example.com/path/(?<b>beta)[^\s]*(\s*)$";
    private const string Input = "   https://benchmark.example.com/path/beta?q=query   ";

    [Params(1, 1000)] public int Repetitions;

    [GlobalSetup]
    public void SetUp()
    {
        _variables = new Dictionary<string, string> { { "a", "-A-" }, { "b", "-B-" }, { "c", "-C-" } };
    }

    [Benchmark(Baseline = true)]
    public string InterpretedExpression()
    {
        ITextTransformer transformer = new RegExTextTransformer(Expression, _variables, RegexOptions.None);
        
        for (int i = 0; i < Repetitions; ++i)
        {
            _ = transformer.Transform(Input);
        }

        return transformer.Transform(Input);
    }

    [Benchmark]
    public string CompiledExpression()
    {
        ITextTransformer transformer = new RegExTextTransformer(Expression, _variables, RegexOptions.Compiled);
        
        for (int i = 0; i < Repetitions; ++i)
        {
            _ = transformer.Transform(Input);
        }

        return transformer.Transform(Input);
    }
}