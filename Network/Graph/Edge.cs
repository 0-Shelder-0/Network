using System;

namespace Network.Graph
{
    public class Edge
    {
        public Node From { get; }
        public Node To { get; }

        public Edge(Node from, Node to)
        {
            From = from;
            To = to;
        }

        public Node GetOtherNode(Node node)
        {
            if (node.Equals(From))
            {
                return To;
            }
            if (node.Equals(To))
            {
                return From;
            }
            throw new ArgumentException();
        }

        public override bool Equals(object obj)
        {
            if (obj is Edge edge)
            {
                if (edge.From.Equals(From) && edge.To.Equals(To))
                {
                    return true;
                }
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(From, To);
        }
    }
}
