using System;
using UnityEngine;

namespace Game.Pathfinding
{
    [Serializable]
    public struct PathfindingRequest
    {
        public Vector3 start;
        public Vector3 goal;
        public Action<Vector3[], bool> callback;

        public PathfindingRequest(Vector3 start, Vector3 goal, Action<Vector3[], bool> callback)
        {
            this.start = start;
            this.goal = goal;
            this.callback = callback;
        }
    }
}