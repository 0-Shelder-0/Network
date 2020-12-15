using System;
using System.Collections.Generic;
using System.Linq;

namespace Network.Graph
{
    public class Graph : IGraph
    {
        private readonly List<Node> _nodes;

        public Graph()
        {
            _nodes = new List<Node>();
        }

        public void AddNode(int number)
        {
            if (ContainsNode(number))
            {
                throw new ArgumentException("This node number already exists!");
            }
            _nodes.Add(new Node(number));
        }

        public bool RemoveNode(int number)
        {
            if (!ContainsNode(number))
            {
                return false;
            }
            var currentNode = GetNode(number);
            foreach (var incidentEdge in currentNode.IncidentEdges())
            {
                var node = incidentEdge.GetOtherNode(currentNode);
                node.Edges.Remove(incidentEdge);
            }
            return _nodes.Remove(currentNode);
        }

        public void Connect(int firstNumber, int secondNumber)
        {
            if (!ContainsNode(firstNumber) || !ContainsNode(secondNumber))
            {
                throw new ArgumentException("The number of two or one of the nodes was not found!");
            }
            var first = GetNode(firstNumber);
            var second = GetNode(secondNumber);
            if (first.IsConnect(second) || second.IsConnect(first))
            {
                throw new ArgumentException("This edge already exists!");
            }
            var edge = new Edge(first, second);
            first.Edges.Add(edge);
            second.Edges.Add(edge);
        }

        public bool Disconnect(int firstNumber, int secondNumber)
        {
            if (!ContainsNode(firstNumber) || !ContainsNode(secondNumber))
            {
                return false;
            }
            var first = GetNode(firstNumber);
            var second = GetNode(secondNumber);
            if (!first.IsConnect(second) || !second.IsConnect(first))
            {
                return false;
            }
            var edge = first.Edges.Find(e => e.First.Equals(second) || e.Second.Equals(second));
            return first.Edges.Remove(edge) && second.Edges.Remove(edge);
        }

        public bool ContainsNode(int number)
        {
            return _nodes.Select(node => node.Number)
                         .Contains(number);
        }

        public bool IsConnect(int firstNumber, int secondNumber)
        {
            var first = GetNode(firstNumber);
            var second = GetNode(secondNumber);
            if (first == null || second == null)
            {
                return false;
            }
            return first.IsConnect(second) && second.IsConnect(first);
        }

        private Node GetNode(int number)
        {
            return _nodes.Find(node => node.Number.Equals(number));
        }
    }
}
