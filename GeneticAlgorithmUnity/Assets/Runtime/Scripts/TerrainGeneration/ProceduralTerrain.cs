using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.ProceduralTerrainGeneration
{
    [System.Serializable]
    public class ProceduralTerrain
    {
        [SerializeField]
        private int _seed;

        private ProceduralTerrainSettings _settings;
        
        private Mesh _mesh;
        private Vector3[] _vertices;
        private int[] _triangles;
        private Vector2[] _uv;
        private Color[] _colors;

        private float _maxHeight;
        private float _minHeight;

        private int _xIndex;
        private int _zIndex;

        public ProceduralTerrain(int seed, ProceduralTerrainSettings settings, int xIndex, int zIndex)
        {
            _seed = seed;
            _settings = settings;
            _xIndex = xIndex;
            _zIndex = zIndex;
        }

        public Mesh GenerateMesh()
        {
            _settings.ClampParameters();

            CreateVertices();
            CreateTriangles();

            CreateColorMap();

            UpdateMesh();

            return _mesh;
        }

        private void CreateVertices()
        {
            int xSize = _settings.XSize;
            int zSize = _settings.ZSize;

            _minHeight = float.MaxValue;
            _maxHeight = float.MinValue;

            _vertices = new Vector3[(xSize + 1) * (zSize + 1)];
            _uv = new Vector2[(xSize + 1) * (zSize + 1)];

            for (int i = 0, z = 0; z <= zSize; z++)
            {
                for (int x = 0; x <= xSize; x++)
                {
                    float y = GetHeight(x, z);

                    _vertices[i] = new Vector3(x, y, z);
                    _uv[i] = new Vector2(x / (float)xSize, z / (float)zSize);
                    SetMinAndMaxHeight(y);

                    i++;
                }
            }
        }

        private void CreateTriangles()
        {
            int xSize = _settings.XSize;
            int zSize = _settings.ZSize;

            _triangles = new int[xSize * zSize * 6];

            int tIndex = 0;
            int vIndex = 0;

            for (int i = 0; i < zSize; i++)
            {
                for (int j = 0; j < xSize; j++)
                {
                    _triangles[tIndex + 0] = vIndex;
                    _triangles[tIndex + 1] = vIndex + xSize + 1;
                    _triangles[tIndex + 2] = vIndex + 1;

                    _triangles[tIndex + 3] = vIndex + 1;
                    _triangles[tIndex + 4] = vIndex + xSize + 1;
                    _triangles[tIndex + 5] = vIndex + xSize + 2;

                    tIndex += 6;
                    vIndex++;
                }

                vIndex++;
            }
        }

        private void CreateColorMap()
        {
            _colors = new Color[_vertices.Length];

            for (int i = 0; i < _vertices.Length; i++)
            {
                float height = Mathf.InverseLerp(_minHeight, _maxHeight, _vertices[i].y);
                _colors[i] = _settings.Gradient.Evaluate(height);
            }
        }

        private void UpdateMesh()
        {
            _mesh = new Mesh();

            _mesh.Clear();

            _mesh.name = "Terrain";

            _mesh.vertices = _vertices;

            _mesh.uv = _uv;
            _mesh.colors = _colors;

            _mesh.triangles = _triangles;

            _mesh.RecalculateNormals();
            _mesh.RecalculateTangents();
        }

        private float GetHeight(int x, int z)
        {
            Vector2[] octaveOffsets = GetOctaveOffsets();

            float amplitude = 1;
            float frequency = 1;
            float noiseHeight = 0;

            for (int i = 0; i < _settings.Octaves; i++)
            {
                float sampleX = (x + (_settings.XSize * _xIndex)) / _settings.Scale * frequency + octaveOffsets[i].x;
                float sampleZ = (z + (_settings.ZSize * _zIndex)) / _settings.Scale * frequency + octaveOffsets[i].y;

                float perlinValue = Mathf.PerlinNoise(sampleX, sampleZ) * 2 - 1;

                noiseHeight += _settings.HeightCurve.Evaluate(perlinValue) * amplitude * _settings.HeightMultiplier;

                amplitude *= _settings.Persistance;
                frequency *= _settings.Lacunarity;
            }

            return noiseHeight;
        }

        private Vector2[] GetOctaveOffsets()
        {
            System.Random pnrg = new System.Random(_seed);
            Vector2[] octaveOffsets = new Vector2[_settings.Octaves];

            for (int i = 0; i < _settings.Octaves; i++)
            {
                float offsetX = pnrg.Next(-100000, 100000) + _settings.Offset.x;
                float offsetY = pnrg.Next(-100000, 100000) + _settings.Offset.y;

                octaveOffsets[i] = new Vector2(offsetX, offsetY);
            }

            return octaveOffsets;
        }

        private void SetMinAndMaxHeight(float height)
        {
            if (height > _maxHeight)
            {
                _maxHeight = height;
            }
            else if (height < _minHeight)
            {
                _minHeight = height;
            }
        }
    } 
}
