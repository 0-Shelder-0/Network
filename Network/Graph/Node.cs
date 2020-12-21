using System.Collections.Generic;
using System.Linq;

namespace Network.Graph
{
    public class Node
    {
        public int Number { get; }
        public readonly Dictionary<int, Edge> Edges;

        public Node(int number)
        {
            Number = number;
            Edges = new Dictionary<int, Edge>();
        }

        public IEnumerable<Edge> IncidentEdges()
        {
            return Edges.Values;
        }

        public IEnumerable<Node> IncidentNodes()
        {
            return Edges.Values.Select(edge => edge.GetOtherNode(this));
        }

        public bool IsConnect(Node node)
        {
            return node.Equals(this) || Edges.ContainsKey(node.Number);
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
            return Number;
        }
    }
}
