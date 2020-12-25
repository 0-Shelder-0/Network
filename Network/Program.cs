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
            var edge1 = new Edge(new Node(1), new Node(0), 4);
            var edge2 = new Edge(new Node(0), new Node(1), 4);
            Console.WriteLine(edge1.GetHashCode());
            Console.WriteLine(edge2.GetHashCode());
            
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
                (3, 4, 1),
                (4, 5, 1)
            };
            network.MakeNetwork(nodes, links);
            router.GetPathsByDijkstra(network, 0, 5);
        }
    }
}
