using System;
using System.Collections.Generic;
using System.Linq;

namespace Network.Graph
{
    public class Node
    {
        public int Number { get; }
        public readonly List<Edge> Edges;

        public Node(int number)
        {
            Number = number;
            Edges = new List<Edge>();
        }

        public IEnumerable<Edge> IncidentEdges()
        {
            return Edges;
        }

        public IEnumerable<Node> IncidentNodes()
        {
            return Edges.Select(edge => edge.GetOtherNode(this));
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
            return Number.GetHashCode();
        }
    }
}
