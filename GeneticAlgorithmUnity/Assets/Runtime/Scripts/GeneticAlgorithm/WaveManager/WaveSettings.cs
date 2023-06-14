using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpawnSettings", menuName = "Settings/SpawnSettings")]
public class WaveSettings : ScriptableObject
{
    public int enemiesPerWave;
    public float waveDuration;
    public float minSpawnInterval;
    public float maxSpawnInterval;
    public float waveRespawnTime;
    public int traitsGivenOnWaveEnd;
}
