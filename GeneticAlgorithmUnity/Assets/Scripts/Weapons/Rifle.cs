using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Projectiles;

namespace Game.Weapons
{
    public class Rifle : Weapon
    {
        [SerializeField]
        private Projectile _projectile;
        [SerializeField]
        private float _cooldown;

        private bool _canFire = true;

        protected override void Awake()
        {
            base.Awake();
            _projectile.owner = gameObject;
        }

        private void Fire(EntityEventContext ctx)
        {
            if (_canFire)
            {
                Instantiate(_projectile, transform.position, transform.rotation);
                StartCoroutine(Recharge());
            }
        }

        private IEnumerator Recharge()
        {
            _canFire = false;

            yield return new WaitForSeconds(_cooldown);

            _canFire = true;
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
