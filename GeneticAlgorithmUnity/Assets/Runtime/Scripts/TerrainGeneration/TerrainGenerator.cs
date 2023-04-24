using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.TerrainGenerator
{
    public class TerrainGenerator : MonoBehaviour
    {
        [Header("General")]
        [SerializeField]
        private int _seed;
        [SerializeField]
        private bool _generateRandomSeed = true;
        [SerializeField]
        private int _xSize = 20;
        [SerializeField]
        private int _zSize = 20;

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

        private Mesh _mesh;
        private Vector3[] _vertices;
        private int[] _triangles;
        private Vector2[] _uv;
        private float[,] _noiseMap;


        private void Start()
        {
            GenerateTerrain();

            if (_generateRandomSeed)
            {
                _seed = Random.Range(-1000, 1000);
            }
        }

        private float GetHeight(int x, int z)
        {
            Vector2[] octaveOffsets = GetOctaveOffsets();

            float halfXSize = _xSize * 0.5f;
            float halfZSize = _zSize * 0.5f;

            float amplitude = 1;
            float frequency = 1;
            float noiseHeight = 0;

            for (int i = 0; i < _octaves; i++)
            {
                float sampleX = (x - halfXSize) / _scale * frequency + octaveOffsets[i].x;
                float sampleZ = (z - halfZSize) / _scale * frequency + octaveOffsets[i].y;

                float perlinValue = Mathf.PerlinNoise(sampleX, sampleZ) * 2 - 1;

                noiseHeight += _heightCurve.Evaluate(perlinValue) * amplitude * _heightMultiplier;

                amplitude *= _persistance;
                frequency *= _lacunarity;
            }

            return noiseHeight;
        }

        private void ClampParameters()
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

        private Vector2[] GetOctaveOffsets()
        {
            System.Random pnrg = new System.Random(_seed);
            Vector2[] octaveOffsets = new Vector2[_octaves];

            for (int i = 0; i < _octaves; i++)
            {
                float offsetX = pnrg.Next(-100000, 100000) + _offset.x;
                float offsetY = pnrg.Next(-100000, 100000) + _offset.y;

                octaveOffsets[i] = new Vector2(offsetX, offsetY);
            }

            return octaveOffsets;
        }

        private void GenerateTerrain()
        {
            ClampParameters();

            CreateVertices();
            CreateTriangles();

            UpdateMesh();
        }

        private void CreateVertices()
        {
            _vertices = new Vector3[(_xSize + 1) * (_zSize + 1)];
            _uv = new Vector2[(_xSize + 1) * (_zSize + 1)];

            for (int i = 0, z = 0; z <= _zSize; z++)
            {
                for (int x = 0; x <= _xSize; x++)
                {
                    _vertices[i] = new Vector3(x, GetHeight(x, z), z);
                    _uv[i] = new Vector2(x / (float)_xSize, z / (float)_zSize);
                    i++;
                }
            }
        }

        private void CreateTriangles()
        {
            _triangles = new int[_xSize * _zSize * 6];

            int tIndex = 0;
            int vIndex = 0;

            for (int i = 0; i < _xSize; i++)
            {
                for (int j = 0; j < _xSize; j++)
                {
                    _triangles[tIndex + 0] = vIndex;
                    _triangles[tIndex + 1] = vIndex + _xSize + 1;
                    _triangles[tIndex + 2] = vIndex + 1;

                    _triangles[tIndex + 3] = vIndex + 1;
                    _triangles[tIndex + 4] = vIndex + _xSize + 1;
                    _triangles[tIndex + 5] = vIndex + _xSize + 2;

                    tIndex += 6;
                    vIndex++;
                }

                vIndex++;
            }

            /*int vert = 0;
            int tris = 0;

            for (int z = 0; z < _terrainSize.x; z++)
            {
                for (int x = 0; x < _terrainSize.x; x++)
                {
                    _triangles[tris + 0] = vert + 0;
                    _triangles[tris + 1] = vert + _terrainSize.x + 1;
                    _triangles[tris + 2] = vert + +1;
                    _triangles[tris + 3] = vert + +1;
                    _triangles[tris + 4] = vert + _terrainSize.x + 1;
                    _triangles[tris + 5] = vert + _terrainSize.x + 2;

                    tris++;
                    vert += 6;
                }

                vert++;
            }*/
        }

        private void UpdateMesh()
        {
            _mesh = new Mesh();
            GetComponent<MeshFilter>().mesh = _mesh;
            
            _mesh.Clear();
            
            _mesh.name = "Terrain";

            _mesh.vertices = _vertices;
            _mesh.triangles = _triangles;
            _mesh.uv = _uv;

            _mesh.RecalculateNormals();
            _mesh.RecalculateTangents();

            GetComponent<MeshCollider>().sharedMesh = _mesh;
        }

        private void OnValidate()
        {
            GenerateTerrain();
        }
    } 
}
