using System;
using System.Collections.Generic;
using System.Linq;

namespace Network.Graph
{
    public class Graph : IGraph
    {
        private readonly Dictionary<int, Node> _nodes;

        public Graph()
        {
            _nodes = new Dictionary<int, Node>();
        }

        public void AddNode(int number)
        {
            if (ContainsNode(number))
            {
                throw new ArgumentException("This node number already exists!");
            }
            _nodes[number] = new Node(number);
        }

        public bool RemoveNode(int number)
        {
            if (!_nodes.ContainsKey(number))
            {
                return false;
            }
            var currentNode = _nodes[number];
            foreach (var incidentNode in currentNode.IncidentNodes())
            {
                incidentNode.Disconnect(this, currentNode);
            }
            return _nodes.Remove(number);
        }

        public void Connect(int firstNumber, int secondNumber, int weight)
        {
            if (!ContainsNode(firstNumber) || !ContainsNode(secondNumber))
            {
                throw new ArgumentException("The number of two or one of the nodes was not found!");
            }
            var first = _nodes[firstNumber];
            var second = _nodes[secondNumber];
            if (first.IsConnect(second) || second.IsConnect(first))
            {
                throw new ArgumentException("This edge already exists!");
            }
            first.Connect(this, second, weight);
        }

        public bool Disconnect(int firstNumber, int secondNumber)
        {
            if (!ContainsNode(firstNumber) || !ContainsNode(secondNumber))
            {
                return false;
            }
            var first = _nodes[firstNumber];
            var second = _nodes[secondNumber];
            return first.Disconnect(this, second);
        }

        public bool ContainsNode(int number)
        {
            return _nodes.ContainsKey(number);
        }

        public bool IsConnect(int firstNumber, int secondNumber)
        {
            if (!ContainsNode(firstNumber) || !ContainsNode(secondNumber))
            {
                return false;
            }
            var first = _nodes[firstNumber];
            var second = _nodes[secondNumber];
            return first.IsConnect(second) && second.IsConnect(first);
        }

        public IEnumerable<Edge> GetEdges()
        {
            return _nodes.Values
                         .SelectMany(edge => edge.IncidentEdges())
                         .Distinct();
        }

        public IEnumerable<Node> GetNodes()
        {
            return _nodes.Values;
        }

        public void Clear()
        {
            _nodes.Clear();
        }

        public Node this[int number]
        {
            get
            {
                if (!ContainsNode(number))
                {
                    throw new ArgumentException("This node number was not found!");
                }
                return _nodes[number];
            }
        }
        public Edge this[int firstNumber, int secondNumber]
        {
            get
            {
                if (!ContainsNode(firstNumber) || !ContainsNode(secondNumber))
                {
                    throw new ArgumentException("The pair of nodes with these numbers was not found!");
                }
                return _nodes[firstNumber][secondNumber];
            }
        }
    }
}
