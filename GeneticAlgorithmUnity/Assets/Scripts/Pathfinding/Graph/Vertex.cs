using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Pathfinding
{
    public class Vertex : IHeapNode<Vertex>
    {
        private Vector3 _position;
        private Dictionary<Vertex, Edge> _edges;
        public Vector3 Position => _position;
        private int _heapIndex;
        public Vertex parent;
        public float gCost;
        public float hCost;
        public float fCost => gCost + hCost;
        public int terrainPenalty;

        public int Index { get => _heapIndex; set => _heapIndex = value; }

        public Vertex(Vector3 position)
        {
            _edges = new Dictionary<Vertex, Edge>();
            _position = position;
        }

        public void ConnectTo(Vertex target)
        {
            _edges.TryAdd(target, new Edge(this, target));
        }

        public Vertex[] GetConnectedVertices()
        {
            Vertex[] connectedVertices = new Vertex[_edges.Count()];

            for (int i = 0; i < _edges.Count; i++)
            {
                connectedVertices[i] = _edges.ElementAt(i).Value.Target;
            }

            return connectedVertices;
        }

        public void RemoveConnectedVertex(Vertex vertex)
        {
            if (_edges.ContainsKey(vertex))
            {
                _edges.Remove(vertex);
            }
        }

        public static Color GetColorBasedOnTerrainType(TerrainType terrainType, int terrainPenalty = 0)
        {
            switch (terrainType)
            {
                case TerrainType.None:
                    return new Color(1, 1, 1, 1);

                case TerrainType.NonWalkable:
                    return new Color(1, 0, 0, 0.4f);

                case TerrainType.Walkable:
                    return new Color(0, 1, 0, 1f / (terrainPenalty + 1));

                default:
                    return new Color(1, 1, 1, 1);
            }
        }

        public int CompareTo(Vertex other)
        {
            int compare = fCost.CompareTo(other.fCost);
            if (compare == 0)
            {
                compare = gCost.CompareTo(other.gCost);
            }

            return -compare;
        }
    }
}