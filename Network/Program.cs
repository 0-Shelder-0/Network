namespace Network
{
    static class Program
    {
        private static void Main()
        {
            RunBenchmark(50);
        }

        private static void RunBenchmark(int numberRepetitions)
        {
            const string path = "C:/Users/Vlad/Desktop/Tests";
            Benchmark.Benchmark.Run(new Graph.Graph(), path, 250, numberRepetitions);
        }
    }
}
