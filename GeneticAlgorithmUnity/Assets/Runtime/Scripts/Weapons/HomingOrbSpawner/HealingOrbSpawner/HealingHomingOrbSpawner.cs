using Game.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Weapons
{
    public class HealingHomingOrbSpawner : HomingOrbSpawner<HealingHomingOrbSpawnerData>
    {
        protected override void SetLayers()
        {
            _hitLayer = _entity.AllyLayer | (1 << Constants.GroundLayer);
        }

        protected override void Fire(ref EntityEventContext ctx)
        {
            if (_canUse)
            {
                ctx.Weapon = new EntityEventContext.WeaponPacket() { RecoilStrength = _data.RecoilStrength };
                ctx.EventController.TriggerEvent(EntityEventType.OnWeaponAttack, ctx);

                if (_data.Amount == 1)
                {
                    Vector3 spawnPosition = transform.position + Vector3.up * 5f;

                    InstantiateAndInitializeHomingOrb(_data.Orb, _entity.AllyLayer, spawnPosition, Quaternion.identity);

                    StartCoroutine(Recharge());
                } else if (_data.Amount > 1)
                {
                    StartCoroutine(SpawnOrbsCoroutine());
                    _canUse = false;
                }
            }
        }

        private IEnumerator SpawnOrbsCoroutine()
        {
            Vector3 spawnPosition = transform.position;
            spawnPosition.y += _data.YDistance;

            for (int i = 0; i < _data.Amount; i++)
            {
                float radians = 2 * Mathf.PI / _data.Amount * i;

                float xDirection = Mathf.Cos(radians);
                float zDirection = Mathf.Sin(radians);

                Vector3 direction = new Vector3(xDirection, 0, zDirection);
                spawnPosition += direction * _data.OrbSpawnRadius;

                InstantiateAndInitializeHomingOrb(_data.Orb, _entity.AllyLayer, spawnPosition, Quaternion.identity);

                yield return new WaitForSeconds(_data.OrbSpawnInterval);
            }

            StartCoroutine(Recharge());
        }
    } 
}
