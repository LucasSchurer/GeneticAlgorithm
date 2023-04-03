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

        public Vector2Int GridSize => _gridSize;
        public float VertexSize => _vertexSize;
        public LayerMask ObstacleLayerMask => _obstacleLayerMask;
        public LayerMask BlockableClearanceLayerMask => _blockableClearanceLayerMask;
        public float Offset => _offset;
    }
}