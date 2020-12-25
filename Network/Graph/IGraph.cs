namespace Network.Graph
{
    public interface IGraph
    {
        void AddNode(int number);
        bool RemoveNode(int number);
        void Connect(int firstNumber, int secondNumber, int weight);
        bool Disconnect(int firstNumber, int secondNumber);
        bool ContainsNode(int number);
        bool IsConnect(int firstNumber, int secondNumber);
        void Clear();

        Node this[int number] { get; }
        Edge this[int firstNumber, int secondNumber] { get; }
    }
}
