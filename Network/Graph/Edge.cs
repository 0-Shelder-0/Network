using System;

namespace Network.Graph
{
    public class Edge
    {
        public Node FirstNode { get; }
        public Node SecondNode { get; }

        public Edge(Node firstNode, Node secondNode)
        {
            FirstNode = firstNode;
            SecondNode = secondNode;
        }

        public Node GetOtherNode(Node node)
        {
            if (node.Equals(FirstNode))
            {
                return SecondNode;
            }
            if (node.Equals(SecondNode))
            {
                return FirstNode;
            }
            throw new ArgumentException();
        }

        public override bool Equals(object obj)
        {
            if (obj is Edge edge)
            {
                if (edge.FirstNode.Equals(FirstNode) && edge.SecondNode.Equals(SecondNode))
                {
                    return true;
                }
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(FirstNode, SecondNode);
        }
    }
}
