using System;
using System.Diagnostics;
using Network.Graph;

namespace Network.Measurer
{
    public class Measurer
    {
        public double Measure(IGraph graph, string methodName, object[] parameters)
        {
            var methodInfo = typeof(IGraph).GetMethod(methodName);
            if (methodInfo == null)
            {
                throw new ArgumentException($"Method {methodName} not exists!");
            }

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            methodInfo.Invoke(graph, parameters);
            stopwatch.Stop();

            return stopwatch.Elapsed.TotalMilliseconds;
        }
    }
}
