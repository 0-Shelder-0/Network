using System;
using System.Collections.Generic;
using System.Linq;
using Network.Graph;
using Network.NetworkNodes;

namespace Network.Network
{
    public class Network : INetwork
    {
        public IGraphCheck GraphOfNetwork => _networkGraph;

        private readonly Dictionary<int, NetworkNode> _networkNodes;
        private readonly IGraph _networkGraph;

        public Network()
        {
            _networkGraph = new Graph.Graph();
            _networkNodes = new Dictionary<int, NetworkNode>();
        }

        public void AddNode(NetworkNode networkNode)
        {
            _networkGraph.AddNode(networkNode.Number);
            _networkNodes[networkNode.Number] = networkNode;
        }

        public bool RemoveNode(int number)
        {
            if (_networkNodes.ContainsKey(number))
            {
                return _networkGraph.RemoveNode(number) && _networkNodes.Remove(number);
            }
            return false;
        }

        public void Connect(int firstNumber, int secondNumber, int weight)
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

        public bool IsConnect(int firstNumber, int secondNumber)
        {
            return _networkGraph.IsConnect(firstNumber, secondNumber);
        }

        public bool Disconnect(int firstNumber, int secondNumber)
        {
            return _networkGraph.Disconnect(firstNumber, secondNumber);
        }

        public bool ContainsNode(int nodeNumber)
        {
            return _networkNodes.ContainsKey(nodeNumber);
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

        private bool IsPossibleAddLink(NetworkNode firstNode, NetworkNode secondNode)
        {
            var endNode = firstNode.Type == NodeType.EndNode ? firstNode : secondNode;
            return !GraphOfNetwork[endNode.Number].IncidentEdges().Any();
        }
    }
}
