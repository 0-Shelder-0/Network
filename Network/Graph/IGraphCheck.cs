using System.Collections.Generic;

namespace Network.Graph
{
    public interface IGraphCheck
    {
        int NumberNodes { get; }
        int NumberEdges { get; }
        
        bool ContainsNode(int number);
        bool IsConnect(int firstNumber, int secondNumber);
        IEnumerable<Edge> GetEdges();
        IEnumerable<Node> GetNodes();

        Node this[int number] { get; }
        Edge this[int firstNumber, int secondNumber] { get; }
    }
}
