namespace Altemiq.Buffers.Compression.Differential;

public class DeltaBenchmarks
{
    public class DeltaForward
    {
        public static IEnumerable<object[]> FowardData()
        {
            var cdg = new ClusteredDataGenerator();
            IEnumerable<int> nbrs = [18];

            foreach (var nbr in nbrs)
            {
                var maxSparsity = 31 - nbr;
                for (var sparsity = 1; sparsity <= maxSparsity; sparsity += 8)
                {
                    var dataSize = 1 << (nbr + sparsity);
                    var data = cdg.GenerateClustered(1 << nbr, dataSize);
                    yield return new object[] { data, sparsity };
                }
            }
        }

        [Benchmark]
        [ArgumentsSource(nameof(FowardData))]
        public void AltemiqForward(int[] data, int sparsity) => Delta.Forward(data);

        [Benchmark(Baseline = true)]
        [ArgumentsSource(nameof(FowardData))]
        public void GenboxForward(int[] data, int sparsity) => Genbox.CSharpFastPFOR.Differential.Delta.delta(data);
    }

    public class DeltaReverse
    {
        public static IEnumerable<object[]> ReverseData()
        {
            IEnumerable<int> nbrs = [18];
            var random = new Random(123456);

            foreach (var nbr in nbrs)
            {
                var maxSparsity = 31 - nbr;
                for (var sparsity = 1; sparsity <= maxSparsity; sparsity += 8)
                {
                    var dataSize = 1 << (nbr + sparsity);

                    var data = new int[dataSize];
                    for (var i = 0; i < dataSize; i++)
                    {
                        data[i] = random.Next(1, 7);
                    }


                    yield return new object[] { data, sparsity };
                }
            }
        }

        [Benchmark]
        [ArgumentsSource(nameof(ReverseData))]
        public void AltemiqReverse(int[] data, int sparsity) => Delta.Inverse(data);

        [Benchmark(Baseline = true)]
        [ArgumentsSource(nameof(ReverseData))]
        public void GenboxReverse(int[] data, int sparsity) => Genbox.CSharpFastPFOR.Differential.Delta.fastinverseDelta(data);
    }
}