using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Network.Graph;

namespace Network.Benchmark
{
    public static class Benchmark
    {
        public static void Run(IGraph graph, string directoryPath, int numberItems, int numberRepetitions)
        {
            var allResults = new List<(string, List<double>)>();
            var nodeNumbers = GetNodeNumbers(numberItems);
            var edges = GetEdges(numberItems);

            for (var i = 0; i < numberRepetitions; i++)
            {
                allResults.AddRange(MeasureTwoMethods(graph, nodeNumbers, "AddNode", "ContainsNode"));
                allResults.AddRange(MeasureTwoMethods(graph, edges, "Connect", "IsConnect"));
                allResults.Add(MeasureMethod(graph, edges, "Disconnect"));
                ConnectAllEdges(graph, edges);
                allResults.Add(MeasureMethod(graph, nodeNumbers, "RemoveNode"));
            }

            var results = CombineResults(allResults);
            WriteToFiles(directoryPath, results);
        }

        private static IEnumerable<(string, List<double>)> MeasureTwoMethods(IGraph graph,
                                                                             IEnumerable<object[]> parametersEnumerable,
                                                                             string firstName,
                                                                             string secondName)
        {
            var measurer = new Measurer.Measurer();
            var firstResults = new List<double>();
            var secondResults = new List<double>();
            foreach (var parameters in parametersEnumerable)
            {
                var result = measurer.Measure(graph, firstName, parameters);
                firstResults.Add(result);

                result = measurer.Measure(graph, secondName, parameters);
                secondResults.Add(result);
            }
            return new[] {(firstName, firstResults), (secondName, secondResults)};
        }

        private static (string, List<double>) MeasureMethod(IGraph graph,
                                                            IEnumerable<object[]> parametersEnumerable,
                                                            string methodName)
        {
            var measurer = new Measurer.Measurer();
            var result = new List<double>();
            foreach (var parameters in parametersEnumerable)
            {
                result.Add(measurer.Measure(graph, methodName, parameters));
            }
            return (methodName, result);
        }

        private static IEnumerable<(string, List<double>)> CombineResults(
            IReadOnlyCollection<(string, List<double>)> results)
        {
            var combineResults = new List<(string, List<double>)>();
            var names = new HashSet<string>(results.Select(item => item.Item1));
            foreach (var name in names)
            {
                combineResults.Add((name, new List<double>()));
                var methodResults = results
                                   .Where(tuple => string.Compare(tuple.Item1, name, StringComparison.Ordinal) == 0)
                                   .ToList();
                for (var i = 1; i < methodResults.FirstOrDefault().Item2.Count; i++)
                {
                    var avg = methodResults.Average(item => item.Item2[i]);
                    combineResults[^1].Item2.Add(avg);
                }
            }
            return combineResults;
        }

        private static void ConnectAllEdges(IGraph graph, IEnumerable<object[]> edges)
        {
            foreach (var parameters in edges)
            {
                if (parameters[0] is int && parameters[1] is int)
                {
                    var item1 = parameters[0] as int? ?? 0;
                    var item2 = parameters[1] as int? ?? 0;
                    graph.Connect(item1, item2);
                }
            }
        }

        private static List<object[]> GetNodeNumbers(int numberItems)
        {
            return Enumerable.Range(0, numberItems)
                             .Select(item => new object[] {item})
                             .ToList();
        }

        private static List<object[]> GetEdges(int count)
        {
            var set = new HashSet<(int, int)>();
            var rnd = new Random();
            while (set.Count != count)
            {
                var edge = (rnd.Next(0, count), rnd.Next(0, count));
                if (edge.Item1 >= edge.Item2)
                {
                    continue;
                }
                set.Add(edge);
            }
            return set.Select(tuple => new object[] {tuple.Item1, tuple.Item2})
                      .ToList();
        }

        private static void WriteToFiles(string directoryPath, IEnumerable<(string, List<double>)> results)
        {
            foreach (var (methodName, methodResults) in results)
            {
                var path = Path.Combine(directoryPath, $"{methodName}.txt");
                using (var fileStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    fileStream.StreamWriteLine(methodName);
                    foreach (var result in methodResults)
                    {
                        fileStream.StreamWriteLine(result.ToString(CultureInfo.InvariantCulture));
                    }
                }
            }
        }
    }
}
