using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Pathfinding
{
    public class Vertex : IHeapNode<Vertex>
    {
        private int _identifier;
        private int _rowIndex;
        private int _columnIndex;
        private Vector3 _position;
        private float _size;
        private Dictionary<int, Edge> _edges;
        private TerrainType _terrainType;
        public Vector3 Position => _position;
        public float Size => _size;
        public int Identifier => _identifier;
        public int RowIndex => _rowIndex;
        public int ColumnIndex => _columnIndex;

        private int _heapIndex;
        public TerrainType TerrainType => _terrainType;

        public Vertex parent;
        public float gCost;
        public float hCost;
        public float fCost => gCost + hCost;
        public int terrainPenalty;

        public int Index { get => _heapIndex; set => _heapIndex = value; }

        public Vertex(int identifier, int rowIndex, int columnIndex, Vector3 position, float size, TerrainType terrainType, int terrainPenalty = 0)
        {
            _edges = new Dictionary<int, Edge>();
            _identifier = identifier;
            _rowIndex = rowIndex;
            _columnIndex = columnIndex;
            _position = position;
            _size = size;
            _terrainType = terrainType;
            this.terrainPenalty = terrainPenalty;
        }

        public void ConnectTo(Vertex target)
        {
            if (!_edges.ContainsKey(target.Identifier) && target.Identifier != Identifier)
            {
                _edges.Add(target.Identifier, new Edge(this, target));
            }
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
            if (_edges.ContainsKey(vertex.Identifier))
            {
                _edges.Remove(vertex.Identifier);
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