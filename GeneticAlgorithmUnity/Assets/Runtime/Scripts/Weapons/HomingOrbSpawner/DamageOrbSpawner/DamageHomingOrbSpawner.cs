using Game.Events;
using Game.Projectiles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Weapons
{
    public class DamageHomingOrbSpawner : HomingOrbSpawner<DamageHomingOrbSpawnerData>
    {
        protected override void SetLayers()
        {
            _hitLayer = _entity.EnemyLayer | (1 << Constants.GroundLayer);
        }

        protected override void Fire(ref EntityEventContext ctx)
        {
            if (_canUse)
            {
                ctx.Weapon = new EntityEventContext.WeaponPacket() { RecoilStrength = _data.RecoilStrength };
                ctx.EventController.TriggerEvent(EntityEventType.OnWeaponAttack, ctx);

                Vector3 spawnPosition = transform.position;

                spawnPosition.x += Random.Range(_data.XSpawnRange.x, _data.XSpawnRange.y);
                spawnPosition.y += Random.Range(_data.YSpawnRange.x, _data.YSpawnRange.y);
                spawnPosition.z += Random.Range(_data.ZSpawnRange.x, _data.ZSpawnRange.y);

                InstantiateAndInitializeHomingOrb(_data.Orb, _entity.EnemyLayer, spawnPosition, Quaternion.identity);

                StartCoroutine(Recharge());
            }
        }
    } 
}
