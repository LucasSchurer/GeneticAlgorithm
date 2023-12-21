using Game.Projectiles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Weapons
{
    public abstract class HomingOrbSpawn : ScriptableObject
    {
        public abstract IEnumerator Spawn(HomingOrbSpawner spawner, HomingOrbSpawnerData data, int bonusProjectiles);

        protected virtual HomingOrb InstantiateAndInitializeHomingOrb(HomingOrbSpawner spawner, HomingOrbSpawnerData data, Vector3 spawnPosition, Quaternion rotation)
        {
            HomingOrb orb = Instantiate(data.Orb, spawnPosition, rotation);

            orb.Initialize(spawner.gameObject, data, data.HitLayer, data.TargetLayer);

            return orb;
        }
    } 
}
