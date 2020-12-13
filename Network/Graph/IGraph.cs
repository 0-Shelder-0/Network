namespace Network.Graph
{
    public interface IGraph
    {
        void AddNode(int number);
        void RemoveNode(int number);
        void AddEdge(int firstNodeNumber, int secondNodeNumber);
        void RemoveEdge(int firstNodeNumber, int secondNodeNumber);
        bool Contains(int number);
    }
}
