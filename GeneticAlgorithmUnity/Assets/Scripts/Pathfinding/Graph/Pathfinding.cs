using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Pathfinding
{
    public class Pathfinding : Singleton<Pathfinding>
    {
        private Queue<PathfindingRequest> _requests;
        private PathfindingRequest _currentRequest;

        private Graph _graph;

        private bool _isProcessingPath = false;

        protected override void SingletonAwake()
        {
            _graph = GetComponent<Graph>();
            _graph.Initialize();
            _requests = new Queue<PathfindingRequest>();
        }

        /// <summary>
        /// Request a path, which will use the A* algorithm to find the shortest path between
        /// the start position and the goal position.
        /// </summary>
        /// <remarks>
        /// It will only work if the start and goal position exists inside the graph.
        /// </remarks>
        /// <param name="startPosition"></param>
        /// <param name="goalPosition"></param>
        /// <param name="callback"></param>
        public void RequestPath(Vector3 startPosition, Vector3 goalPosition, Action<Vector3[], bool> callback)
        {
            Vertex startVertex = _graph.GetVertexOnPosition(startPosition);
            Vertex goalVertex = _graph.GetVertexOnPosition(goalPosition);

            if (startVertex != null && goalVertex != null)
            {
                PathfindingRequest request = new PathfindingRequest(startVertex, goalVertex, callback);
                _requests.Enqueue(request);
                TryProcessNextPath();
            }
            else
            {
                callback(null, false);
            }
        }

        private void TryProcessNextPath()
        {
            if (!_isProcessingPath && _requests.Count > 0)
            {
                _currentRequest = _requests.Dequeue();
                _isProcessingPath = true;
                StartCoroutine(FindPathCoroutine(_currentRequest.start.Identifier, _currentRequest.goal.Identifier));
            }
        }

        private void OnPathProcessed(Vector3[] path, bool success)
        {
            _currentRequest.callback(path, success);
            _isProcessingPath = false;
            TryProcessNextPath();
        }

        /// <summary>
        /// A* Algorithm Implementation to find a path between two vertices
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        private IEnumerator FindPathCoroutine(int startIdentifier, int goalIdentifier)
        {
            FindPath(startIdentifier, goalIdentifier);
            yield return null;
        }

        private void FindPath(int startIdentifier, int goalIdentifier)
        {
            if (startIdentifier == goalIdentifier)
            {
                OnPathProcessed(null, false);
                return;
            }

            Vertex start;
            Vertex goal;

            if (!_graph.Vertices.TryGetValue(startIdentifier, out start) || !_graph.Vertices.TryGetValue(goalIdentifier, out goal))
            {
                OnPathProcessed(null, false);
                return;
            }

            bool foundPath = false;
            HashSet<Vertex> closedSet = new HashSet<Vertex>();
            Heap<Vertex> openSet = new Heap<Vertex>(_graph.VerticesCount);
            Vertex[] steps = null;
            Vertex currentVertex;

            start.hCost = DistanceBetweenVertices(start, goal);
            start.gCost = 0;
            start.parent = null;
            openSet.Add(start);

            while (openSet.Count > 0)
            {
                currentVertex = openSet.RemoveFirst();
                closedSet.Add(currentVertex);

                if (currentVertex == goal)
                {
                    steps = RetraceSteps(goal);
                    foundPath = true;
                    break;
                }

                foreach (Vertex connectedVertex in currentVertex.GetConnectedVertices())
                {
                    if (closedSet.Contains(connectedVertex))
                    {
                        continue;
                    }

                    int movementCostToConnectedVertex = currentVertex.gCost + DistanceBetweenVertices(currentVertex, connectedVertex) + connectedVertex.terrainPenalty;
                    if (!openSet.Contains(connectedVertex) || movementCostToConnectedVertex < connectedVertex.gCost)
                    {
                        connectedVertex.gCost = movementCostToConnectedVertex;
                        connectedVertex.hCost = DistanceBetweenVertices(connectedVertex, goal);
                        connectedVertex.parent = currentVertex;

                        if (!openSet.Contains(connectedVertex))
                        {
                            openSet.Add(connectedVertex);
                        }
                        else
                        {
                            openSet.UpdateNode(connectedVertex);
                        }
                    }
                }
            }

            OnPathProcessed(steps, foundPath);
        }

        /// <summary>
        /// Return a array with all the vertices used to reach a target vertex.
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        private Vertex[] RetraceSteps(Vertex target)
        {
            List<Vertex> steps = new List<Vertex>();

            while (target != null)
            {
                steps.Add(target);
                target = target.parent;
            }

            steps.Reverse();
            return steps.ToArray();
        }

        private int DistanceBetweenVertices(Vertex a, Vertex b)
        {
            int xDistance = Mathf.Abs(a.ColumnIndex - b.ColumnIndex);
            int yDistance = Mathf.Abs(a.RowIndex - b.RowIndex);

            return 14 * Mathf.Min(xDistance, yDistance) + 10 * Mathf.Abs(xDistance - yDistance);
        }
    }
}