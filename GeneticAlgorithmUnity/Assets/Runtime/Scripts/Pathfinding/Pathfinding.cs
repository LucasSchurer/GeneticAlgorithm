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
            PathfindingRequest request = new PathfindingRequest(startPosition, goalPosition, callback);
            _requests.Enqueue(request);
            TryProcessNextPath();
        }

        private void TryProcessNextPath()
        {
            if (!_isProcessingPath && _requests.Count > 0)
            {
                _currentRequest = _requests.Dequeue();
                _isProcessingPath = true;
                StartCoroutine(FindPathCoroutine(_currentRequest.start, _currentRequest.goal));
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
        private IEnumerator FindPathCoroutine(Vector3 startPosition, Vector3 goalPosition)
        {
            FindPath(startPosition, goalPosition);
            yield return null;
        }

        private void FindPath(Vector3 startPosition, Vector3 goalPosition)
        {
            Vertex start = _graph.CreateVertexAndEdges(startPosition);
            Vertex goal = _graph.CreateVertexAndEdges(goalPosition);

            bool foundPath = false;
            HashSet<Vertex> closedSet = new HashSet<Vertex>();
            Heap<Vertex> openSet = new Heap<Vertex>(_graph.VerticesCount);
            Vector3[] steps = null;
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

                    float movementCostToConnectedVertex = currentVertex.gCost + DistanceBetweenVertices(currentVertex, connectedVertex);
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

            _graph.RemoveVertexAndEdges(start);
            _graph.RemoveVertexAndEdges(goal);

            OnPathProcessed(steps, foundPath);
        }

        /// <summary>
        /// Return a array with all the vertices used to reach a target vertex.
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        private Vector3[] RetraceSteps(Vertex target)
        {
            List<Vector3> steps = new List<Vector3>();

            while (target != null)
            {
                steps.Add(target.Position);
                target = target.parent;
            }

            steps = SimplifySteps(steps);
            steps.Reverse();
            return steps.ToArray();
        }

        private List<Vector3> SimplifySteps(List<Vector3> steps)
        {
            List<Vector3> simplifiedSteps = new List<Vector3>();
            Vector3 oldDirection = Vector3.zero;

            for (int i = 0; i < steps.Count; i++)
            {
                if (i == steps.Count - 1)
                {
                    simplifiedSteps.Add(steps[i]);
                    continue;
                }

                Vector3 newDirection = (steps[i + 1] - steps[i]).normalized;
                
                if (newDirection != oldDirection)
                {
                    simplifiedSteps.Add(steps[i]);
                }

                oldDirection = newDirection;
            }

            return simplifiedSteps;
        }

        private float DistanceBetweenVertices(Vertex a, Vertex b)
        {
            return Vector3.Distance(a.Position, b.Position);
        }
    }
}