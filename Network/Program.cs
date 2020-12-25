using System.Collections.Generic;
using Network.NetworkNodes;

namespace Network
{
    static class Program
    {
        private static void Main()
        {
            var network = new Network.Network(new Graph.Graph());
            var nodes = new List<NetworkNode>
            {
                new EndNode(0),
                new Router(1),
                new Router(2),
                new Router(3),
                new Router(4),
                new EndNode(5)
            };
            var links = new List<(int, int)>
            {
                (0, 1),
                (1, 2),
                (2, 3),
                (3, 4),
                (4, 5)
            };
            network.MakeNetwork(nodes, links);
        }
    }
}
