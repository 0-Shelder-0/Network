using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
                new EndNode(9),
                new EndNode(10)
            };
            var links = new List<(int, int, int)>
            {
                (0, 1, 1),
                (1, 2, 1),
                (1, 3, 2),
                (1, 4, 3),
                (2, 3, 1),
                (2, 4, 2),
                (3, 4, 1),
                (4, 9, 1),
                (4, 10, 1)
            };
            network.MakeNetwork(nodes, links);
            var s = new Stopwatch();
            router.GetPathByDijkstra(network, 1, 10);
            router.GetPathByBellmanFord(network, 1, 10);
            
            s.Start();
            var dijkstra = router.GetPathByDijkstra(network, 0, 10);
            s.Stop();
            Console.WriteLine(s.Elapsed);
            
            s.Restart();
            var bellmanFord = router.GetPathByBellmanFord(network, 0, 10);
            s.Stop();
            Console.WriteLine(s.Elapsed);
            
            Console.WriteLine(dijkstra.Time == bellmanFord.Time);
            Console.WriteLine(dijkstra.Path.SequenceEqual(bellmanFord.Path));
        }
    }
}
