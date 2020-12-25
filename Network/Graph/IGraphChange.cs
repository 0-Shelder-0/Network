namespace Network.Graph
{
    public interface IGraphChange
    {
        void AddNode(int number);
        bool RemoveNode(int number);
        void Connect(int firstNumber, int secondNumber, int weight);
        bool Disconnect(int firstNumber, int secondNumber);
        void Clear();
    }
}
