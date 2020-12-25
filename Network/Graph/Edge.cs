using System;

namespace Network.Graph
{
    public class Edge
    {
        public Node First { get; }
        public Node Second { get; }
        public int Weight { get; }

        public Edge(Node first, Node second, int weight)
        {
            First = first;
            Second = second;
            Weight = weight;
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
                if (edge.Weight.Equals(Weight) &&
                    (edge.First.Equals(First) && edge.Second.Equals(Second) ||
                     edge.First.Equals(Second) && edge.Second.Equals(First)))
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
                return First.GetHashCode() + Second.GetHashCode() + HashCode.Combine(Weight);
            }
        }
    }
}
