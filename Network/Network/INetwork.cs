using System.Collections.Generic;
using Network.Graph;
using Network.NetworkNodes;

namespace Network.Network
{
    public interface INetwork
    {
        IGraphCheck GraphOfNetwork { get; }

        void MakeNetwork(IEnumerable<NetworkNode> networkNodes,
                         IEnumerable<(int FirstNode, int SecondNode, int Distance)> links);

        public NetworkNode this[int index] { get; }
    }
}
