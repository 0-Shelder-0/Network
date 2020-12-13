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
            if (Contains(number))
            {
                throw new ArgumentException("This number already exists!");
            }
            _nodes.Add(new Node(number));
        }

        public void RemoveNode(int number)
        {
            var currentNode = GetNode(number);
            if (currentNode == null)
            {
                throw new ArgumentException("This number not found!");
            }
            foreach (var incidentEdge in currentNode.IncidentEdges())
            {
                var node = incidentEdge.GetOtherNode(currentNode);
                node.Edges.Remove(incidentEdge);
            }
            _nodes.Remove(currentNode);
        }

        public void AddEdge(int fromNumber, int toNumber)
        {
            var from = GetNode(fromNumber);
            var to = GetNode(toNumber);
            if (from == null || to == null)
            {
                throw new ArgumentException("The number of two or one of the nodes was not found");
            }
            var edge = new Edge(from, to);
            if (from.Edges.Contains(edge) || to.Edges.Contains(edge))
            {
                throw new ArgumentException("This edge already exists!");
            }
            from.Edges.Add(edge);
            to.Edges.Add(edge);
        }

        public void RemoveEdge(int fromNumber, int toNumber)
        {
            var node = GetNode(fromNumber);
            var edge = node?.Edges.Find(e => e.To.Number == toNumber);
            if (edge == null)
            {
                throw new ArgumentException("The number of two or one of the nodes was not found");
            }
            edge.From.Edges.Remove(edge);
            edge.To.Edges.Remove(edge);
        }

        public bool Contains(int number)
        {
            return _nodes.Select(node => node.Number)
                         .Contains(number);
        }

        private Node GetNode(int number)
        {
            return _nodes.Find(node => node.Number.Equals(number));
        }
    }
}
