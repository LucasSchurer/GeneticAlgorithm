using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.ProceduralTerrainGeneration
{
    public class ProceduralTerrainGenerator : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField]
        private ProceduralTerrainSettings _settings = new ProceduralTerrainSettings();

        [Header("General")]
        [SerializeField]
        private int _seed;
        [SerializeField]
        private bool _generateRandomSeed = true;
        [SerializeField]
        [Range(1, 10)]
        private int _xSize = 1;
        [SerializeField]
        [Range(1, 10)]
        private int _zSize = 1;

        [Header("Debug")]
        [SerializeField]
        private bool _generateAtStart = true;
        [SerializeField]
        private bool _autoUpdate = false;

        private ProceduralTerrain[,] _terrains;
        private Queue<GameObject> _childrenQueue;

        private void Start()
        {
            if (_generateRandomSeed)
            {
                _seed = Random.Range(-1000, 1000);
            }

            if (_generateAtStart)
            {
                GenerateTerrains();
            }
        }

        private void GenerateTerrains()
        {
            BuildChildrenQueue();

            _terrains = new ProceduralTerrain[_xSize, _zSize];

            for (int z = 0; z < _zSize; z++)
            {
                for (int x = 0; x < _xSize; x++)
                {
                    GenerateTerrain(x, z);
                }
            }
        }

        private void GenerateTerrain(int x, int z)
        {
            GameObject go = RequestChild(x, z);

            _terrains[x, z] = new ProceduralTerrain(_seed, _settings, x, z);

            Mesh mesh = _terrains[x, z].GenerateMesh();

            go.GetComponent<MeshFilter>().mesh = mesh;
            go.GetComponent<MeshCollider>().sharedMesh = mesh;
            go.GetComponent<MeshRenderer>().material = _settings.Material;
        }

        private void BuildChildrenQueue()
        {
            if (_childrenQueue != null)
            {
                _childrenQueue.Clear();
            }
            else
            {
                _childrenQueue = new Queue<GameObject>();
            }

            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
                _childrenQueue.Enqueue(child.gameObject);
            }
        }

        private GameObject RequestChild(int x, int z)
        {
            GameObject child;

            if (_childrenQueue.Count > 0)
            {
                child = _childrenQueue.Dequeue();
            } else
            {
                child = CreateTerrainGameObject();
            }

            child.name = "Terrain" + x.ToString() + z.ToString();

            child.SetActive(true);

            Vector3 position = Vector3.zero;

            position.x = _settings.XSize * x;
            position.z = _settings.ZSize * z;

            child.transform.localPosition = position;

            child.gameObject.layer = gameObject.layer;

            return child;
        }

        private GameObject CreateTerrainGameObject()
        {
            GameObject go = new GameObject("Terrain", typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider));
            go.transform.SetParent(transform);
            go.transform.position = Vector3.zero;

            return go;
        }

        private void OnValidate()
        {
            if (_autoUpdate)
            {
                GenerateTerrains();
            }
        }
    } 
}
