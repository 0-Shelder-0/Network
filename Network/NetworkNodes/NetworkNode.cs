using System;

namespace Network.NetworkNodes
{
    public abstract class NetworkNode
    {
        public int Number { get; }
        public NodeType Type { get; }

        protected NetworkNode(int number, NodeType type)
        {
            Number = number;
            Type = type;
        }

        public override bool Equals(object obj)
        {
            if (obj is NetworkNode networkNode)
            {
                if (networkNode.Number.Equals(Number) && networkNode.Type == Type)
                {
                    return true;
                }
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Number, Type);
        }
    }
}
