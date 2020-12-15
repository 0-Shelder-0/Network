namespace Network.Graph
{
    public interface IGraph
    {
        void AddNode(int number);
        bool RemoveNode(int number);
        void Connect(int firstNumber, int secondNumber);
        bool Disconnect(int firstNumber, int secondNumber);
        bool ContainsNode(int number);
        bool IsConnect(int firstNumber, int secondNumber);
    }
}
