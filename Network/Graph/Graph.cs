using System;
using System.Collections.Generic;
using System.Linq;

namespace Network.Graph
{
    public class Graph : IGraph
    {
        private readonly List<Node> _nodes;
        private readonly List<Edge> _edges;

        public Graph()
        {
            _nodes = new List<Node>();
            _edges = new List<Edge>();
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
                _edges.Remove(incidentEdge);
                foreach (var incidentNode in currentNode.IncidentNodes())
                {
                    incidentNode.Edges.Remove(incidentEdge);
                }
            }
            _nodes.Remove(currentNode);
        }

        public void AddEdge(int firstNodeNumber, int secondNodeNumber)
        {
            var firstNode = GetNode(firstNodeNumber);
            var secondNode = GetNode(secondNodeNumber);
            if (firstNode == null || secondNode == null)
            {
                throw new ArgumentException("The number of two or one of the nodes was not found");
            }
            var edge = new Edge(firstNode, secondNode);
            if (_edges.Contains(edge))
            {
                throw new ArgumentException("This edge already exists!");
            }
            firstNode.Edges.Add(edge);
            secondNode.Edges.Add(edge);
            _edges.Add(edge);
        }

        public void RemoveEdge(int firstNodeNumber, int secondNodeNumber)
        {
            var edge = GetEdge(firstNodeNumber, secondNodeNumber);
            if (edge == null)
            {
                throw new ArgumentException("The number of two or one of the nodes was not found");
            }
            _edges.Remove(edge);
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

        private Edge GetEdge(int firstNodeNumber, int secondNodeNumber)
        {
            return _edges.Find(edge => edge.FirstNode.Number.Equals(firstNodeNumber) &&
                                       edge.SecondNode.Number.Equals(secondNodeNumber));
        }
    }
}
