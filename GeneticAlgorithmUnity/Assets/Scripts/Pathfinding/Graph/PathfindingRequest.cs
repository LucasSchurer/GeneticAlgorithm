using System;

namespace Game.Pathfinding
{
    [Serializable]
    public struct PathfindingRequest
    {
        public Vertex start;
        public Vertex goal;
        public Action<Vertex[], bool> callback;

        public PathfindingRequest(Vertex start, Vertex goal, Action<Vertex[], bool> callback)
        {
            this.start = start;
            this.goal = goal;
            this.callback = callback;
        }
    }
}