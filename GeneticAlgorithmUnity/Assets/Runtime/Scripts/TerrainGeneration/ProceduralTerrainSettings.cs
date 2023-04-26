using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.ProceduralTerrainGeneration
{
    [System.Serializable]
    public class ProceduralTerrainSettings 
    {
        [Header("General")]
        [SerializeField]
        private int _seed;
        [SerializeField]
        private int _xSize = 20;
        [SerializeField]
        private int _zSize = 20;
        [SerializeField]
        private Gradient _gradient;
        public Gradient Gradient => _gradient;

        [Header("Height")]
        [SerializeField]
        private float _scale = 1;
        [SerializeField]
        [Range(1, 15)]
        private int _octaves = 1;
        [SerializeField]
        [Range(0f, 1f)]
        private float _persistance = 1;
        [SerializeField]
        private float _lacunarity = 1;
        [SerializeField]
        private Vector2 _offset;
        [SerializeField]
        private AnimationCurve _heightCurve;

        [SerializeField]
        private float _heightMultiplier = 2f;

        #region Getters and Setters
        private int Seed { get => _seed; set => _seed = value; }
        public int XSize { get => _xSize; set => _xSize = value; }
        public int ZSize { get => _zSize; set => _zSize = value; }
        public float Scale { get => _scale; set => _scale = value; }
        public int Octaves { get => _octaves; set => _octaves = value; }
        public float Persistance { get => _persistance; set => _persistance = value; }
        public float Lacunarity { get => _lacunarity; set => _lacunarity = value; }
        public Vector2 Offset { get => _offset; set => _offset = value; }
        public AnimationCurve HeightCurve => _heightCurve;
        public float HeightMultiplier { get => _heightMultiplier; set => _heightMultiplier = value; }
        #endregion

        public void ClampParameters()
        {
            if (_scale <= 0)
            {
                _scale = 0.0001f;
            }

            if (_octaves < 0)
            {
                _octaves = 0;
            }
        }
    }
}
