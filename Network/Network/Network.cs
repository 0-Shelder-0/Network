using System;
using System.Collections.Generic;
using System.Linq;
using Network.Graph;
using Network.NetworkNodes;

namespace Network.Network
{
    public class Network
    {
        private readonly IGraph _graphOfNetwork;
        private readonly Dictionary<int, NetworkNode> _networkNodes;

        public Network(IGraph graph)
        {
            _graphOfNetwork = graph;
            _networkNodes = new Dictionary<int, NetworkNode>();
        }

        public void MakeNetwork(IEnumerable<NetworkNode> networkNodes, IEnumerable<(int, int)> links)
        {
            foreach (var networkNode in networkNodes)
            {
                AddNode(networkNode);
            }

            foreach (var (firstNode, secondNode) in links)
            {
                Connect(firstNode, secondNode);
            }
        }

        private void AddNode(NetworkNode networkNode)
        {
            _networkNodes[networkNode.Number] = networkNode;
            _graphOfNetwork.AddNode(networkNode.Number);
        }

        private void Connect(int firstNumber, int secondNumber)
        {
            if (!ContainsNode(firstNumber) || !ContainsNode(secondNumber))
            {
                throw new ArgumentException("The number of two or one of the nodes was not found!");
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
            _graphOfNetwork.Connect(firstNumber, secondNumber);
        }

        private bool ContainsNode(int nodeNumber)
        {
            return _networkNodes.ContainsKey(nodeNumber);
        }

        private bool IsPossibleAddLink(NetworkNode firstNode, NetworkNode secondNode)
        {
            var endNode = firstNode.Type == NodeType.EndNode ? firstNode : secondNode;
            return !_graphOfNetwork[endNode.Number].IncidentEdges().Any();
        }
    }
}
