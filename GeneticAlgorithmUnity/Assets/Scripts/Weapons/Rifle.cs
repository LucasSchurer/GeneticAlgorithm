using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Projectiles;
using Game.Events;

namespace Game.Weapons
{
    public class Rifle : Weapon<ScriptableObjects.RangedWeapon>
    {
        private Projectile _projectile;

        protected override void Awake()
        {
            base.Awake();
            _projectile = _settings.baseProjectile;
            _canUse = true;
        }

        private void Fire(ref EntityEventContext ctx)
        {
            if (_canUse)
            {
                if (_settings.projectileVariation > 0)
                {
                    float yAngle = Random.Range(transform.rotation.eulerAngles.y - _settings.projectileVariation, transform.rotation.eulerAngles.y + _settings.projectileVariation);
                    Quaternion projectileDirection = Quaternion.Euler(transform.rotation.eulerAngles.x, yAngle, transform.rotation.eulerAngles.z);
                    Projectile projectile = Instantiate(_projectile, transform.position, projectileDirection);
                    projectile.Instantiate(gameObject, _settings.damage);
                } else
                {
                    Projectile projectile = Instantiate(_projectile, transform.position, transform.rotation);
                    projectile.Instantiate(gameObject, _settings.damage);
                }
                
                StartCoroutine(Recharge());
            }
        }

        private IEnumerator Recharge()
        {
            _canUse = false;

            yield return new WaitForSeconds(_settings.cooldown);

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
    } 
}
