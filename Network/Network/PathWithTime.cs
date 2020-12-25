using System.Collections.Generic;

namespace Network.Network
{
    public class PathWithTime
    {
        public List<int> Path { get; }
        public int Time { get; }

        public PathWithTime(List<int> path, int time)
        {
            Path = path;
            Time = time;
        }
    }
}
