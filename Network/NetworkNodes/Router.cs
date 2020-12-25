using System;
using System.Collections.Generic;
using Network.Graph;

namespace Network.NetworkNodes
{
    public class Router : NetworkNode
    {
        public Router(int number) : base(number, NodeType.Router) { }

        public IEnumerable<int> GetNetworkPath(IGraph networkStructure, int nodeNumber)
        {
            if (!networkStructure.ContainsNode(Number))
            {
                throw new ArgumentException();
            }
            return null;
        }

        public IEnumerable<int> GetPath(IGraph networkStructure)
        {
            return null;
        }
    }
}
