using System;
using System.Collections.Generic;
using System.Linq;
using Network.Graph;
using Network.NetworkNodes;

namespace Network.Network
{
    public class Network : INetwork
    {
        private readonly Dictionary<int, NetworkNode> _networkNodes;
        private readonly IGraph _networkGraph;

        public IGraphCheck GraphOfNetwork => _networkGraph;

        public Network(IGraph networkGraph)
        {
            _networkGraph = networkGraph;
            _networkNodes = new Dictionary<int, NetworkNode>();
        }

        public void MakeNetwork(IEnumerable<NetworkNode> networkNodes,
                                IEnumerable<(int FirstNode, int SecondNode, int Distance)> links)
        {
            foreach (var networkNode in networkNodes)
            {
                AddNode(networkNode);
            }

            foreach (var (firstNode, secondNode, weight) in links)
            {
                Connect(firstNode, secondNode, weight);
            }
        }

        public NetworkNode this[int index]
        {
            get
            {
                if (!_networkNodes.ContainsKey(index))
                {
                    throw new ArgumentException("This host number was not found!");
                }
                return _networkNodes[index];
            }
        }

        private void AddNode(NetworkNode networkNode)
        {
            _networkNodes[networkNode.Number] = networkNode;
            _networkGraph.AddNode(networkNode.Number);
        }

        private void Connect(int firstNumber, int secondNumber, int weight)
        {
            if (!ContainsNode(firstNumber) || !ContainsNode(secondNumber))
            {
                throw new ArgumentException("The number of two or one of the nodes was not found!");
            }
            if (weight < 0)
            {
                throw new ArgumentException("The distance parameter must be non-negative!");
            }
            var firstNode = _networkNodes[firstNumber];
            var secondNode = _networkNodes[secondNumber];
            if (firstNode.Type == NodeType.EndNode && secondNode.Type == NodeType.EndNode)
            {
                throw new ArgumentException("End node can only be connected to a router!");
            }
            if (firstNode.Type == NodeType.EndNode || secondNode.Type == NodeType.EndNode)
            {
                if (!IsPossibleAddLink(firstNode, secondNode))
                {
                    throw new ArgumentException("The end node of the network cannot have more than 1 connection!");
                }
            }
            _networkGraph.Connect(firstNumber, secondNumber, weight);
        }

        private bool ContainsNode(int nodeNumber)
        {
            return _networkNodes.ContainsKey(nodeNumber);
        }

        private bool IsPossibleAddLink(NetworkNode firstNode, NetworkNode secondNode)
        {
            var endNode = firstNode.Type == NodeType.EndNode ? firstNode : secondNode;
            return !GraphOfNetwork[endNode.Number].IncidentEdges().Any();
        }
    }
}
