using System.Collections;
using UnityEngine;
using Game.Projectiles;
using Game.Events;

namespace Game.Weapons
{
    public class Rifle : Weapon<RangedWeaponData>
    {
        private Projectile _projectile;

        protected override void Awake()
        {
            base.Awake();
            _projectile = _data.baseProjectile;
            _canUse = true;
        }

        protected override void SetSocketsAndVFXs()
        {
        }

        private void Fire(ref EntityEventContext ctx)
        {
            if (_canUse)
            {
                if (_data.projectileVariation > 0)
                {
                    float yAngle = Random.Range(transform.rotation.eulerAngles.y - _data.projectileVariation, transform.rotation.eulerAngles.y + _data.projectileVariation);
                    Quaternion projectileDirection = Quaternion.Euler(transform.rotation.eulerAngles.x, yAngle, transform.rotation.eulerAngles.z);
                    Projectile projectile = Instantiate(_projectile, transform.position, projectileDirection);
                    projectile.Instantiate(gameObject, _data.Damage);
                } else
                {
                    Projectile projectile = Instantiate(_projectile, ctx.Origin, Quaternion.LookRotation(ctx.Direction, Vector3.up));
                    projectile.Instantiate(gameObject, _data.Damage);
                }
                
                StartCoroutine(Recharge());
            }
        }

        private IEnumerator Recharge()
        {
            _canUse = false;

            yield return new WaitForSeconds(_data.Cooldown);

            _canUse = true;
        }

        public override void StartListening()
        {
            base.StartListening();

            _eventController?.AddListener(EntityEventType.OnPrimaryActionPerformed, Fire);
        }

        public override void StopListening()
        {
            base.StopListening();

            _eventController?.RemoveListener(EntityEventType.OnPrimaryActionPerformed, Fire);
        }

        protected override void SetLayers()
        {
        }
    } 
}
