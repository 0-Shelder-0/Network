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

        public PathWithTime GetPathByDijkstra(INetwork network, int fromNumber, int toNumber)
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
            var track = new Dictionary<int, TrackData> {[fromNumber] = new TrackData(0, fromNumber)};
            var toOpen = GetNodeNumberWithMinCost(track, visited);
            while (toOpen.HasValue)
            {
                if (toOpen.Value == toNumber)
                {
                    return RestorePath(track, fromNumber, toOpen.Value);
                }
                AddCostToNextNode(network.GraphOfNetwork, visited, track, toOpen.Value);
                toOpen = GetNodeNumberWithMinCost(track, visited);
            }
            return new PathWithTime(new List<int>(), int.MaxValue);
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
            var track = network.GraphOfNetwork.GetNodes()
                               .ToDictionary(node => node.Number, value => new TrackData(int.MaxValue, 0));
            track[startNode].Cost = 0;

            for (var pathLength = 1; pathLength < network.GraphOfNetwork.NumberNodes; pathLength++)
            {
                foreach (var edge in network.GraphOfNetwork.GetEdges())
                {
                    CalculateOptimum(track, edge.First.Number, edge.Second.Number, edge.Weight);
                    CalculateOptimum(track, edge.Second.Number, edge.First.Number, edge.Weight);
                }
            }
            return track[finalNode].Cost == int.MaxValue
                       ? new PathWithTime(new List<int>(), int.MaxValue)
                       : RestorePath(track, startNode, finalNode);
        }

        private PathWithTime RestorePath(Dictionary<int, TrackData> track, int start, int end)
        {
            var path = new List<int>();
            var time = track[end].Cost;
            while (end != start)
            {
                path.Add(end);
                end = track[end].Previous;
            }
            path.Add(start);
            path.Reverse();

            return new PathWithTime(path, time);
        }

        private void CalculateOptimum(Dictionary<int, TrackData> track, int first, int second, int weight)
        {
            if (track[first].Cost != int.MaxValue && track[first].Cost + weight < track[second].Cost)
            {
                track[second] = new TrackData(track[first].Cost + weight, first);
            }
        }

        private int? GetNodeNumberWithMinCost(Dictionary<int, TrackData> track, HashSet<int> visited)
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
                                       Dictionary<int, TrackData> track,
                                       int previous)
        {
            foreach (var node in graph[previous].IncidentNodes())
            {
                var cost = track[previous].Cost + graph[previous, node.Number].Weight;
                if (!track.ContainsKey(node.Number) || track[node.Number].Cost > cost)
                {
                    track[node.Number] = new TrackData(cost, previous);
                }
            }
            visited.Add(previous);
        }

        private bool ContainsItems(INetwork network, int fromNumber, int toNumber)
        {
            return network.GraphOfNetwork.ContainsNode(Number) &&
                   network.GraphOfNetwork.ContainsNode(fromNumber) &&
                   network.GraphOfNetwork.ContainsNode(toNumber);
        }

        private bool IsBestCost(Dictionary<int, TrackData> track, int nodeNumber, int bestPrice, HashSet<int> visited)
        {
            return !visited.Contains(nodeNumber) && track[nodeNumber].Cost < bestPrice;
        }

        private class TrackData
        {
            public int Cost { get; set; }
            public int Previous { get; set; }

            public TrackData(int cost, int previous)
            {
                Cost = cost;
                Previous = previous;
            }
        }
    }
}
