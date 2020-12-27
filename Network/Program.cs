using System;
using System.Collections.Generic;
using Network.Graph;
using Network.NetworkNodes;

namespace Network
{
    static class Program
    {
        private static void Main()
        {
            var network = new Network.Network(new Graph.Graph());
            var router = new Router(1);
            var nodes = new List<NetworkNode>
            {
                new EndNode(0),
                router,
                new Router(2),
                new Router(3),
                new Router(4),
                new EndNode(5)
            };
            var links = new List<(int, int, int)>
            {
                (0, 1, 1),
                (1, 2, 1),
                (2, 3, 1),
                (1, 3, 1),
                (3, 4, 1),
                (4, 5, 1)
            };
            network.MakeNetwork(nodes, links);
            var dijkstra = router.GetPathsByDijkstra(network, 0, 5);
            var bellmanFord = router.GetPathByBellmanFord(network, 0, 5);
        }
    }
}
