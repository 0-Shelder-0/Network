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
            
            g.Connect(1, 2);
            g.Connect(1, 3);
            g.Connect(2, 3);
            
            g.Disconnect(2, 1);
            g.Disconnect(1, 3);
            g.Disconnect(2, 3);

            g.Disconnect(1, 1);
        }
    }
}
