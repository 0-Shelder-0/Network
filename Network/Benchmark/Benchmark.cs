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
        public static void Run(IGraph graph, string directoryPath, int numberOfItems,
                               int numberRepetitions)
        {
            numberOfItems++;
            var allResults = new List<(string, List<double>)>();
            var nodeNumbers = GetNodeNumbers(numberOfItems);
            var edges = GetEdges(numberOfItems, numberOfItems);
            var rnd = new Random();
            graph.Clear();

            for (var i = 0; i < numberRepetitions; i++)
            {
                var numberEdges = rnd.Next(1, numberOfItems * (numberOfItems - 1) / 2);
                allResults.AddRange(MeasurePairMethods(graph, nodeNumbers, "AddNode", "ContainsNode"));
                allResults.AddRange(MeasurePairMethods(graph, edges, "Connect", "IsConnect"));
                allResults.Add(MeasureMethod(graph, edges, "Disconnect"));
                ConnectEdges(graph, GetEdges(numberOfItems, numberEdges));
                allResults.Add(MeasureMethod(graph, nodeNumbers, "RemoveNode"));
                allResults[^1].Item2.Reverse();
            }

            var results = CombineResults(allResults);
            WriteToFiles(directoryPath, results);
        }

        private static IEnumerable<(string, List<double>)> MeasurePairMethods(IGraph graph,
                                                                              IEnumerable<object[]> methodsParameters,
                                                                              string firstMethodName,
                                                                              string secondMethodName)
        {
            var measurer = new Measurer.Measurer();
            var firstMethodResults = new List<double>();
            var secondMethodResults = new List<double>();
            foreach (var parameters in methodsParameters)
            {
                var result = measurer.Measure(graph, firstMethodName, parameters);
                firstMethodResults.Add(result);

                result = measurer.Measure(graph, secondMethodName, parameters);
                secondMethodResults.Add(result);
            }
            return new[] {(firstMethodName, firstMethodResults), (secondMethodName, secondMethodResults)};
        }

        private static (string, List<double>) MeasureMethod(IGraph graph,
                                                            IEnumerable<object[]> methodParameters,
                                                            string methodName)
        {
            var measurer = new Measurer.Measurer();
            var result = new List<double>();
            foreach (var parameters in methodParameters)
            {
                result.Add(measurer.Measure(graph, methodName, parameters));
            }
            return (methodName, result);
        }

        private static IEnumerable<(string, List<double>)> CombineResults(List<(string, List<double>)> results)
        {
            var combineResults = new List<(string, List<double>)>();
            var namesOfMethods = new HashSet<string>(results.Select(item => item.Item1));
            foreach (var name in namesOfMethods)
            {
                combineResults.Add((name, new List<double>()));
                var methodResults = results.Where(tuple => name.Equals(tuple.Item1)).ToList();
                var numberOfResults = methodResults.FirstOrDefault().Item2.Count;
                for (var i = 1; i < numberOfResults; i++)
                {
                    var avg = methodResults.Average(item => item.Item2[i]);
                    combineResults[^1].Item2.Add(avg);
                }
            }
            return combineResults;
        }

        private static void ConnectEdges(IGraph graph, IEnumerable<object[]> edges)
        {
            foreach (var parameters in edges)
            {
                if (parameters[0] is int && parameters[1] is int)
                {
                    var item1 = parameters[0] as int? ?? 0;
                    var item2 = parameters[1] as int? ?? 0;
                    graph.Connect(item1, item2, 0);
                }
            }
        }

        private static List<object[]> GetNodeNumbers(int count)
        {
            return Enumerable.Range(0, count)
                             .Select(item => new object[] {item})
                             .ToList();
        }

        private static List<object[]> GetEdges(int maxValue, int count)
        {
            var set = new HashSet<(int, int)>();
            var rnd = new Random();
            while (set.Count != count)
            {
                var edge = (rnd.Next(0, maxValue), rnd.Next(0, maxValue));
                if (edge.Item1 < edge.Item2)
                {
                    set.Add(edge);
                }
            }
            return set.Select(tuple => new object[] {tuple.Item1, tuple.Item2}).ToList();
        }

        private static void WriteToFiles(string directoryPath, IEnumerable<(string, List<double>)> results)
        {
            foreach (var (methodName, methodResults) in results)
            {
                var path = Path.Combine(directoryPath, $"{methodName}.txt");
                using (var fileStream = new FileStream(path, FileMode.Create, FileAccess.Write))
                {
                    fileStream.StreamWriteLine(methodName);
                    foreach (var result in methodResults)
                    {
                        fileStream.StreamWriteLine(result.ToString(CultureInfo.CurrentCulture));
                    }
                }
            }
        }
    }
}
