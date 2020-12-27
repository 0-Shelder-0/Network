using System;
using System.Collections.Generic;
using System.Linq;
using Network.Graph;
using Network.Network;

namespace Network.NetworkNodes
{
    public class Router : NetworkNode
    {
        public Router(int number) : base(number, NodeType.Router) { }

        public PathWithTime GetPathsByDijkstra(INetwork network, int fromNumber, int toNumber)
        {
            if (!ContainsItems(network, fromNumber, toNumber))
            {
                throw new ArgumentException("Not all elements are contained in the graph!");
            }
            if (!network.GraphOfNetwork.IsConnect(fromNumber, Number))
            {
                throw new ArgumentException($"Node number {fromNumber} is not connected to the current router!");
            }
            var visited = new HashSet<int>();
            var track = new Dictionary<int, (int Cost, int PreNumber)> {[fromNumber] = (0, fromNumber)};
            while (true)
            {
                var toOpen = GetNodeNumberWithMinCost(track, visited);
                if (!toOpen.HasValue)
                {
                    break;
                }
                if (toOpen.Value == toNumber)
                {
                    return RestorePath(fromNumber, toOpen.Value, track);
                }
                AddCostToNextNode(network.GraphOfNetwork, visited, track, toOpen.Value);
            }
            throw new Exception("Path not found!");
        }

        public PathWithTime GetPathByBellmanFord(INetwork network, int startNode, int finalNode)
        {
            if (!ContainsItems(network, startNode, finalNode))
            {
                throw new ArgumentException("Not all elements are contained in the graph!");
            }
            if (!network.GraphOfNetwork.IsConnect(startNode, Number))
            {
                throw new ArgumentException($"Node number {startNode} is not connected to the current router!");
            }
            var edges = network.GraphOfNetwork.GetEdges().ToList();
            var maxNodeIndex = network.GraphOfNetwork.GetNodes()
                                      .Select(n => n.Number)
                                      .Max();

            var opt = Enumerable.Repeat(int.MaxValue, maxNodeIndex + 1).ToArray();
            opt[startNode] = 0;

            for (var pathSize = 1; pathSize <= maxNodeIndex; pathSize++)
            {
                foreach (var edge in edges)
                {
                    if (opt[edge.First.Number] != int.MaxValue)
                    {
                        opt[edge.Second.Number] =
                            Math.Min(opt[edge.First.Number] + edge.Weight, opt[edge.Second.Number]);
                    }
                }
            }
            return new PathWithTime(new List<int>(), opt[finalNode]);
        }

        private int? GetNodeNumberWithMinCost(Dictionary<int, (int Cost, int PreNumber)> track, HashSet<int> visited)
        {
            int? toOpen = null;
            var bestPrice = int.MaxValue;
            foreach (var number in track.Keys.Where(num => IsBestCost(track, num, bestPrice, visited)))
            {
                bestPrice = track[number].Cost;
                toOpen = number;
            }
            return toOpen;
        }

        private void AddCostToNextNode(IGraphCheck graph,
                                       HashSet<int> visited,
                                       Dictionary<int, (int Cost, int PreNumber)> track,
                                       int preNodeNumber)
        {
            foreach (var node in graph[preNodeNumber].IncidentNodes())
            {
                var cost = track[preNodeNumber].Cost + graph[preNodeNumber, node.Number].Weight;
                if (!track.ContainsKey(node.Number) || track[node.Number].Cost > cost)
                {
                    track[node.Number] = (cost, preNodeNumber);
                }
            }
            visited.Add(preNodeNumber);
        }

        private bool ContainsItems(INetwork network, int fromNumber, int toNumber)
        {
            return network.GraphOfNetwork.ContainsNode(Number) &&
                   network.GraphOfNetwork.ContainsNode(fromNumber) &&
                   network.GraphOfNetwork.ContainsNode(toNumber);
        }

        private PathWithTime RestorePath(int start, int end, Dictionary<int, (int Cost, int PreNumber)> track)
        {
            var path = new List<int>();
            var time = track[end].Cost;
            while (end != start)
            {
                path.Add(end);
                end = track[end].PreNumber;
            }
            path.Add(start);
            path.Reverse();

            return new PathWithTime(path, time);
        }

        private bool IsBestCost(IReadOnlyDictionary<int, (int Cost, int PreNumber)> track,
                                int nodeNumber,
                                int bestPrice,
                                ICollection<int> visited)
        {
            return !visited.Contains(nodeNumber) && track[nodeNumber].Cost < bestPrice;
        }
    }
}
