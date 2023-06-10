using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Weapons
{
    [CreateAssetMenu(menuName = Constants.WeaponDataMenuName + "/HomingOrbCircularSpawn")]
    public class HomingOrbCircularSpawn : HomingOrbSpawn
    {
        [SerializeField]
        private Vector3 _spawnOffset;
        [SerializeField]
        private float _orbSpawnRadius;

        public override IEnumerator Spawn(HomingOrbSpawner spawner, HomingOrbSpawnerData data)
        {
            Vector3 spawnPosition = spawner.transform.position;
            spawnPosition.y += _spawnOffset.y;

            for (int i = 0; i < data.OrbAmount; i++)
            {
                float radians = 2 * Mathf.PI / data.OrbAmount * i;

                float xDirection = Mathf.Cos(radians);
                float zDirection = Mathf.Sin(radians);

                Vector3 direction = new Vector3(xDirection, 0, zDirection);
                spawnPosition += direction * _orbSpawnRadius;

                InstantiateAndInitializeHomingOrb(spawner, data, spawnPosition, Quaternion.identity);

                if (i == data.OrbAmount - 1)
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
