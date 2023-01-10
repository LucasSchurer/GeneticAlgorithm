using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        DontDestroyOnLoad(gameObject);
    }

    private WaveSettings _waveSettings;
    private Game.GA.PopulationManager _populationManager;
    [SerializeField]
    private Transform _spawnPosition;

    private float _waveTimeRemaining = 0;
    private float _enemiesSpawnRemaining = 0;

    private float _timeBetweenSpawns = 1f;

    private bool _isSpawning = false;

    public void Initialize(WaveSettings waveSettings, Game.GA.PopulationManager populationManager)
    {
        _waveSettings = waveSettings;
        _populationManager = populationManager;
    }

    private Vector3 GetSpawnPosition()
    {
        Vector3 position = _spawnPosition.position;
        position.x += Random.Range(-10f, 10f);
        position.z += Random.Range(-10f, 10f);

        return position;
    }

    public void StartWave()
    {
        _waveTimeRemaining = _waveSettings.waveDuration;
        _enemiesSpawnRemaining = _waveSettings.enemiesPerWave;

        _isSpawning = true;

        StartCoroutine(SpawnCoroutine());
    }

    private void Update()
    {
        if (_isSpawning)
        {
            _waveTimeRemaining -= Time.deltaTime;
        }
    }

    private IEnumerator SpawnCoroutine()
    {
        while (true)
        {
            Game.GA.CreatureController creature = _populationManager.RequestCreature();

            if (creature != null)
            {
                creature.transform.position = GetSpawnPosition();
                creature.gameObject.SetActive(true);

                _enemiesSpawnRemaining--;

                yield return new WaitForSeconds(_timeBetweenSpawns);
            } else
            {
                break;
            }

            if (_enemiesSpawnRemaining <= 0)
            {
                break;
            }
        }

        yield return null;
    }
}
