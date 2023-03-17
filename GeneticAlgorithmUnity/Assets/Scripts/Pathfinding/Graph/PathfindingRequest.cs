using System;
using UnityEngine;

namespace Game.Pathfinding
{
    [Serializable]
    public struct PathfindingRequest
    {
        public Vertex start;
        public Vertex goal;
        public Action<Vector3[], bool> callback;

        public PathfindingRequest(Vertex start, Vertex goal, Action<Vector3[], bool> callback)
        {
            this.start = start;
            this.goal = goal;
            this.callback = callback;
        }
    }
}