namespace Network
{
    static class Program
    {
        private static void Main()
        {
            var g = new Graph.Graph();
            g.AddNode(1);
            g.AddNode(2);
            g.AddNode(3);
            g.AddEdge(1, 2);
            g.AddEdge(2, 3);
            g.RemoveNode(2);
        }
    }
}
