using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Pathfinding
{
    public class Graph : MonoBehaviour
    {
        [SerializeField]
        private MapSettings _settings;
        [SerializeField]
        private Transform _centerTransform;
        public Dictionary<Vector3, Vertex> Vertices => _vertices;
        public int VerticesCount => _vertices.Count;

        private bool _hasInitialized = false;

        private Dictionary<Vector3, Vertex> _vertices;

        private void Awake()
        {
            Initialize();
        }

        public void Initialize(bool forceInitialize = false)
        {
            if (!_hasInitialized || forceInitialize)
            {
                _vertices = new Dictionary<Vector3, Vertex>();
                
                GenerateGraph();

                _hasInitialized = true;
            }
        }

        private void GenerateGraph()
        {
            CreateVertices();
            CreateEdges();
        }

        /// <summary>
        /// Cast a OverlapBox, adding the corners of the colliders returned
        /// by the cast as vertices.
        /// </summary>
        private void CreateVertices()
        {
            Collider[] obstacles = Physics.OverlapBox(_centerTransform.position, new Vector3(_settings.GridSize.x/2, 10f, _settings.GridSize.y/2), Quaternion.identity, _settings.ObstacleLayerMask);

            foreach (Collider obstacle in obstacles)
            {
                BoxCollider collider = obstacle.GetComponent<BoxCollider>();

                if (collider)
                {
                    Vector3[] corners = new Vector3[4];
                    Vector3 center = collider.center;
                    Vector3 size = collider.size;
                    Vector3 extents = size * 0.5f;

                    int index = 0;
                    for (int i = -1; i < 2; i+=2)
                    {
                        for (int j = -1; j < 2; j+=2)
                        {
                            float x = extents.x * i;
                            float z = extents.z * j;

                            Vector3 point = center + new Vector3(x, 0, z);
                            corners[index] = collider.transform.TransformPoint(point);

                            corners[index] += collider.transform.rotation * new Vector3(i, 0, j) * _settings.Offset;
                            corners[index].y = 0.5f;
                            
                            index++;
                        }
                    }

                    foreach (Vector3 corner in corners)
                    {
                        Collider[] c = Physics.OverlapBox(corner, new Vector3(_settings.VertexSize, _settings.VertexSize, _settings.VertexSize)/2, collider.transform.rotation, _settings.ObstacleLayerMask);

                        if (c.Length > 0)
                        {
                            continue;
                        }

                        Vertex vertex = new Vertex(corner);

                        _vertices.TryAdd(corner, vertex);
                    }
                }
            }
        }

        public Vertex CreateVertexAndEdges(Vector3 position)
        {
            if (_vertices.TryGetValue(position, out Vertex vertex))
            {
                return vertex;
            } else
            {
                Vertex newVertex = new Vertex(position);

                _vertices.Add(position, newVertex);

                foreach (Vertex target in _vertices.Values)
                {
                    if (HasClearance(newVertex, target))
                    {
                        newVertex.ConnectTo(target);
                        target.ConnectTo(newVertex);
                    }
                }

                return newVertex;
            }
        }

        public void RemoveVertexAndEdges(Vertex vertex)
        {
            foreach (Vertex connection in vertex.GetConnectedVertices())
            {
                vertex.RemoveConnectedVertex(connection);
                connection.RemoveConnectedVertex(vertex);
            }

            _vertices.Remove(vertex.Position);
        }

        private void CreateEdges()
        {
            foreach (Vertex vertex in _vertices.Values)
            {
                foreach (Vertex target in _vertices.Values)
                {
                    if (vertex.Position == target.Position)
                    {
                        continue;
                    }

                    if (HasClearance(vertex, target))
                    {
                        vertex.ConnectTo(target);
                    }
                }
            }
        }

        private bool HasClearance(Vertex a, Vertex b)
        {
            Vector3 direction = (b.Position - a.Position).normalized;
            float distance = Vector3.Distance(a.Position, b.Position);

            if (Physics.Raycast(a.Position, direction, distance, _settings.BlockableClearanceLayerMask))
            {
                return false;
            }

            return true;
        }

        private void OnDrawGizmosSelected()
        {
            if (_settings && _centerTransform)
            {
                if (Application.isPlaying)
                {
                    if (_vertices != null)
                    {
                        foreach (Vertex vertex in _vertices.Values)
                        {
                            Gizmos.color = Color.green;
                            Gizmos.DrawWireSphere(vertex.Position, 0.15f);

                            foreach (Vertex connectedVertex in vertex.GetConnectedVertices())
                            {
                                Gizmos.color = Color.blue;
                                Gizmos.DrawLine(vertex.Position, connectedVertex.Position);
                            }
                        }
                    }
                }

                // Draw Bounds
                Gizmos.color = Color.red;
                Gizmos.DrawWireCube(_centerTransform.position, new Vector3(_settings.GridSize.x, _centerTransform.position.y, _settings.GridSize.y));
            }
        }
    } 
}
