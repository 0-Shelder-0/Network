namespace Network.Graph
{
    public interface IGraph
    {
        void AddNode(int number);
        void RemoveNode(int number);
        void AddEdge(int fromNumber, int toNumber);
        void RemoveEdge(int fromNumber, int toNumber);
        bool Contains(int number);
    }
}
