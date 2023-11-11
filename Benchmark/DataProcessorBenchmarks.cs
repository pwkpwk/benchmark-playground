using AmbientBytes.Algorithms;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;

namespace AmbientBytes.Benchmark;

[HardwareCounters(HardwareCounter.BranchMispredictions, HardwareCounter.BranchInstructions, HardwareCounter.CacheMisses)]
public class DataProcessorBenchmarks
{
    private Random? _random;
    private List<Entity>? _input;
    private List<Entity>? _existingData;
    private IDataProcessor? _pavlik;
    private IDataProcessor? _andrew;
    
    [Params(10, 50, 100)] public int InputSize;
    [Params(0, 15, 30)] public int ExistingDataSize;
    [Params(false, true)] public bool Shuffle;
    
    [GlobalSetup]
    public void SetUp()
    {
        _random = new Random(InputSize * (ExistingDataSize + 1) + (Shuffle ? 3 : 7));
        _input = new List<Entity>(InputSize);
        _existingData = new List<Entity>(ExistingDataSize);

        long id = _random.NextInt64(1L, 10L);

        for (int i = 0; i < InputSize; ++i)
        {
            _input.Add(new Entity(id, _random.NextInt64(), DateTime.Now));
            id += _random.NextInt64(1L, 5L);
        }

        if (Shuffle)
        {
            ShuffleList(_input);
        }

        for (int i = 0; i < ExistingDataSize; ++i)
        {
            _existingData.Add(new Entity(id, _random.NextInt64(), DateTime.Now));
            id += _random.NextInt64(1L, 5L);
        }

        if (Shuffle)
        {
            ShuffleList(_existingData);
        }

        _pavlik = new Pavlik();
        _andrew = new Andrew();
    }

    [Benchmark]
    public void Pavlik()
    {
        _pavlik!.ProcessData(_input, _input[_input.Count / 3].Id, IDataProcessor.Order.Ascending,
            InputSize + ExistingDataSize, _existingData);
    }

    [Benchmark]
    public void Andrew()
    {
        _andrew!.ProcessData(_input, _input[_input.Count / 3].Id, IDataProcessor.Order.Ascending,
            InputSize + ExistingDataSize, _existingData);
    }

    private void ShuffleList(List<Entity> data)
    {
        for (int i = 0; i < data.Count - 1; ++i)
        {
            if (_random.Next(10) < 4)
            {
                int index = i + _random.Next(data.Count - i);

                if (index > i)
                {
                    (data[i], data[index]) = (data[index], data[i]);
                }
            }
        }
    }
}