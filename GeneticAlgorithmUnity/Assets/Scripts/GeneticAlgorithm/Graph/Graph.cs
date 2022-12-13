using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.GA
{
    public class Graph
    {
        private List<Dictionary<int, Vertex>> _vertices;
        private int _currentVertexId = 0;

        public int CurrentGeneration => _vertices.Count;

        public Graph()
        {
            _vertices = new List<Dictionary<int, Vertex>>();
        }

        public void AddVertex(Vertex vertex)
        {
            if (vertex.Generation >= _vertices.Count)
            {
                _vertices.Add(new Dictionary<int, Vertex>());
            }

            _vertices[vertex.Generation].TryAdd(vertex.Id, vertex);
        }

        public Dictionary<int, Vertex> GetGeneration(int generation)
        {
            if (generation < _vertices.Count)
            {
                return _vertices[generation];
            } else
            {
                return null;
            }
        }

        public Vertex GetVertex(int generation, int id)
        {
            return _vertices[generation][id];
        }
    } 
}
