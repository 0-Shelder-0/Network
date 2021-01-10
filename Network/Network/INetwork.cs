using System.Collections.Generic;
using Network.Graph;
using Network.NetworkNodes;

namespace Network.Network
{
    public interface INetwork
    {
        IGraphCheck GraphOfNetwork { get; }

        void AddNode(NetworkNode networkNode);
        bool RemoveNode(int number);
        void Connect(int firstNumber, int secondNumber, int length);
        bool Disconnect(int firstNumber, int secondNumber);
        bool ContainsNode(int nodeNumber);
        bool IsConnect(int firstNumber, int secondNumber);

        void MakeNetwork(IEnumerable<NetworkNode> networkNodes,
                         IEnumerable<(int FirstNode, int SecondNode, int Distance)> links);

        public NetworkNode this[int index] { get; }
    }
}
