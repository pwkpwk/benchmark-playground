using BenchmarkDotNet.Attributes;

namespace AmbientBytes.Benchmark;

public class TaskBenchmarks
{
    [Benchmark]
    public async Task AwaitInAsync()
    {
        await DoubleAsync();
    }

    [Benchmark]
    public async Task AwaitInAsyncConfigureAwait()
    {
        await DoubleAsync().ConfigureAwait(false);
    }

    [Benchmark]
    public async Task ReturnTask()
    {
        await NoAsync();
    }

    private async Task DoubleAsync()
    {
        await Task.Delay(0);
    }

    private Task NoAsync()
    {
        return Task.Delay(0);
    }
}