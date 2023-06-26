using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Weapons
{
    [CreateAssetMenu(menuName = Constants.WeaponDataMenuName + "/HomingOrbSimpleSpawn")]
    public class HomingOrbSimpleSpawn : HomingOrbSpawn
    {
        [Header("Orb Spawn Settings")]
        [SerializeField]
        private Vector2 _ySpawnRange;
        [SerializeField]
        private Vector2 _zSpawnRange;
        [SerializeField]
        private Vector2 _xSpawnRange;

        public override IEnumerator Spawn(HomingOrbSpawner spawner, HomingOrbSpawnerData data, int bonusProjectiles)
        {
            for (int i = 0; i < data.OrbAmount + bonusProjectiles; i++)
            {
                Vector3 spawnPosition = spawner.transform.position;

                spawnPosition.x += Random.Range(_xSpawnRange.x, _xSpawnRange.y);
                spawnPosition.y += Random.Range(_ySpawnRange.x, _ySpawnRange.y);
                spawnPosition.z += Random.Range(_zSpawnRange.x, _zSpawnRange.y);

                InstantiateAndInitializeHomingOrb(spawner, data, spawnPosition, Quaternion.identity);

                if (i == data.OrbAmount -1)
                {
                    break; 
                } else
                {
                    yield return new WaitForSeconds(data.SpawnInterval);
                }
            }
        }
    } 
}
