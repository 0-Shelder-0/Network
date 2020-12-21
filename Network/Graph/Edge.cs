using System;

namespace Network.Graph
{
    public class Edge
    {
        public Node First { get; }
        public Node Second { get; }

        public Edge(Node first, Node second)
        {
            First = first;
            Second = second;
        }

        public Node GetOtherNode(Node node)
        {
            if (node.Equals(First))
            {
                return Second;
            }
            if (node.Equals(Second))
            {
                return First;
            }
            throw new ArgumentException();
        }

        public override bool Equals(object obj)
        {
            if (obj is Edge edge)
            {
                if (edge.First.Equals(First) && edge.Second.Equals(Second)
                    || edge.First.Equals(Second) && edge.Second.Equals(First))
                {
                    return true;
                }
            }
            return false;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return First.GetHashCode() + Second.GetHashCode();
            }
        }
    }
}
