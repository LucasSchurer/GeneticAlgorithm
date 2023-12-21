using Cinemachine;
using Game.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Entities.Player
{
    public class Recoil : MonoBehaviour, IEventListener
    {
        [SerializeField]
        private EntityEventController _eventController;

        private CinemachineImpulseSource _impulseSource;

        private void Awake()
        {
            _impulseSource = GetComponent<CinemachineImpulseSource>();
        }

        private void OnWeaponAttack(ref EntityEventContext ctx)
        {
            _impulseSource.GenerateImpulse(ctx.Weapon.RecoilStrength);
        }

        public void StartListening()
        {
            if (_eventController)
            {
                _eventController.AddListener(EntityEventType.OnWeaponAttack, OnWeaponAttack, EventExecutionOrder.Standard);
            }
        }

        public void StopListening()
        {
            if (_eventController)
            {
                _eventController.RemoveListener(EntityEventType.OnWeaponAttack, OnWeaponAttack, EventExecutionOrder.Standard);
            }
        }

        private void OnEnable()
        {
            StartListening();
        }

        private void OnDisable()
        {
            StopListening();
        }
    } 
}
