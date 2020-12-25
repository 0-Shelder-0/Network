namespace Network.Graph
{
    public interface IGraphCheck
    {
        bool ContainsNode(int number);
        bool IsConnect(int firstNumber, int secondNumber);

        Node this[int number] { get; }
        Edge this[int firstNumber, int secondNumber] { get; }
    }
}
