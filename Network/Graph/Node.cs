using System;
using System.Collections.Generic;
using System.Linq;

namespace Network.Graph
{
    public class Node
    {
        public int Number { get; }
        private readonly Dictionary<int, Edge> _edges;

        public Node(int number)
        {
            Number = number;
            _edges = new Dictionary<int, Edge>();
        }

        public IEnumerable<Edge> IncidentEdges()
        {
            return _edges.Values;
        }

        public IEnumerable<Node> IncidentNodes()
        {
            return _edges.Values.Select(edge => edge.GetOtherNode(this));
        }

        public Edge Connect(IGraph graph, Node node, int weight)
        {
            if (!graph.ContainsNode(Number) || !graph.ContainsNode(node.Number))
            {
                throw new ArgumentException("This graph contains no current nodes!");
            }
            var edge = new Edge(this, node, weight);
            _edges[node.Number] = edge;
            node._edges[Number] = edge;
            return edge;
        }

        public bool Disconnect(Node node)
        {
            return _edges.Remove(node.Number) && node._edges.Remove(Number);
        }

        public bool IsConnect(Node node)
        {
            return node.Equals(this) || _edges.ContainsKey(node.Number);
        }

        public Edge this[int otherNumber]
        {
            get
            {
                if (!_edges.ContainsKey(otherNumber))
                {
                    throw new ArgumentException();
                }
                return _edges[otherNumber];
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is Node node)
            {
                if (node.Number.Equals(Number))
                {
                    return true;
                }
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Number);
        }
    }
}
