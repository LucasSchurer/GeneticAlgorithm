using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Weapons
{
    [CreateAssetMenu(menuName = Constants.WeaponDataMenuName + "/OrbLauncherSpawn")]
    public class OrbLauncherSpawn : HomingOrbSpawn
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
                Vector3 spawnPosition = spawner.WeaponFireSocket.position;

                float x = Random.Range(_xSpawnRange.x, _xSpawnRange.y);
                float y = Random.Range(_ySpawnRange.x, _ySpawnRange.y);
                float z = Random.Range(_zSpawnRange.x, _zSpawnRange.y);

                spawnPosition += (spawner.WeaponFireSocket.rotation * new Vector3(x, y, z));

                InstantiateAndInitializeHomingOrb(spawner, data, spawnPosition, spawner.WeaponFireSocket.rotation);

                if (i == data.OrbAmount - 1)
                {
                    break;
                }
                else
                {
                    yield return new WaitForSeconds(data.SpawnInterval);
                }

            }
        }
    }
}
