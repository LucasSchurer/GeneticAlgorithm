using System.Collections.Generic;
using UnityEngine;

namespace Game.Pathfinding
{
    [CreateAssetMenu]
    public class MapSettings : ScriptableObject
    {
        [SerializeField]
        private Vector2Int _gridSize;
        [SerializeField]
        private float _vertexSize;
        [SerializeField]
        private LayerMask _obstacleLayerMask;
        [SerializeField]
        private LayerMask _blockableClearanceLayerMask;
        [SerializeField]
        private float _offset;

        [System.Serializable]
        public struct TerrainSettings
        {
            public LayerMask layerMask;
            public TerrainType terrainType;
            public int terrainPenalty;
        }

        [SerializeField]
        private List<TerrainSettings> _terrainSettings;

        public Vector2Int GridSize => _gridSize;
        public float VertexSize => _vertexSize;
        public LayerMask ObstacleLayerMask => _obstacleLayerMask;
        public LayerMask BlockableClearanceLayerMask => _blockableClearanceLayerMask;
        public float Offset => _offset;

        public List<TerrainSettings> GetTerrainSettings => _terrainSettings;
    }
}